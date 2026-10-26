''' <summary>
''' Shared open-redirect guard for every "continue to this page" / "return to this page after
''' sign-in" style redirect in the portal (the sign-out continue link, the password-reset
''' continue link, the post-login ReturnUrl, the dashboard Quick Links panel, and the Admin
''' Toolkit quick-jump box). A redirect target is accepted only when it is a local, relative path
''' inside this application -- never an absolute URL, a scheme-relative ("//host/...") address,
''' or anything else that could send a visitor off this site.
''' </summary>
Public Module RedirectTargetGuard

    ''' <summary>
    ''' True only when target is a same-application relative path: it must start with "~/" or
    ''' "/" (so ResolveUrl/the browser keeps it on this host), must not start with "//" (a
    ''' scheme-relative URL, which a browser still treats as a cross-host redirect), and must not
    ''' contain "://" anywhere (rules out an absolute URL smuggled in after a leading slash, e.g.
    ''' "/\evil.com" style tricks some browsers normalize).
    ''' </summary>
    Public Function IsLocalRedirectTarget(ByVal target As String) As Boolean
        If String.IsNullOrEmpty(target) Then Return False
        ' A browser (per the WHATWG URL spec's very first parsing step) strips every ASCII tab,
        ' CR and LF from a URL before any other processing -- including before the backslash/
        ' slash normalization the checks below exist to catch. A target smuggling one of those
        ' into the middle (e.g. "/" & Chr(9) & "\evil.com", reachable via a URL-encoded
        ' querystring value ASP.NET decodes before this function ever sees it) would otherwise
        ' pass every check below and only turn into the rejected "/\evil.com" shape after the
        ' browser's own normalization. Reject any control character outright.
        For Each c As Char In target
            If AscW(c) < 32 Then Return False
        Next
        If target.StartsWith("//") Then Return False
        ' A bare leading "/" immediately followed by a backslash ("/\evil.com") is left
        ' untouched by the "//" check above, but a browser's URL parser treats a backslash the
        ' same as a forward slash for http(s) -- so this normalizes to "//evil.com", a
        ' scheme-relative address that still leaves this site. Reject it the same way. The
        ' "~/"-prefixed form needs the identical check: ResolveUrl/Redirect only collapse to a
        ' single leading slash when the application has a non-trivial virtual path -- an app
        ' hosted at the IIS site root resolves "~/\evil.com" to exactly "/\evil.com",
        ' reproducing the same bug.
        If target.StartsWith("/\") OrElse target.StartsWith("~/\") Then Return False
        If target.IndexOf("://", StringComparison.OrdinalIgnoreCase) >= 0 Then Return False
        If Not (target.StartsWith("~/") OrElse target.StartsWith("/")) Then Return False
        Return True
    End Function

End Module
