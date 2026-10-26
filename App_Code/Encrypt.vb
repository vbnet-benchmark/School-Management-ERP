Imports Microsoft.VisualBasic
Imports System.Security.Cryptography
Imports System.Text

Public Class Encryption
    Public Function GenerateRandomKey() As Byte()
        Using aes As New AesCryptoServiceProvider()
            aes.GenerateKey()
            Return aes.Key
        End Using
    End Function

    Public Function Encrypt(referenceNumber As String, key As Byte()) As String
        Dim plainTextBytes As Byte() = Encoding.UTF8.GetBytes(referenceNumber)

        Using aes As New AesCryptoServiceProvider()
            aes.Key = key
            aes.Mode = CipherMode.ECB
            aes.Padding = PaddingMode.PKCS7

            Using encryptor As ICryptoTransform = aes.CreateEncryptor()
                Dim cipherTextBytes As Byte() = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length)
                Return Convert.ToBase64String(cipherTextBytes)
            End Using
        End Using
    End Function

    Public Function Decrypt(encryptedReferenceNumber As String, key As Byte()) As String
        Dim cipherTextBytes As Byte() = Convert.FromBase64String(encryptedReferenceNumber)

        Using aes As New AesCryptoServiceProvider()
            aes.Key = key
            aes.Mode = CipherMode.ECB
            aes.Padding = PaddingMode.PKCS7

            Using decryptor As ICryptoTransform = aes.CreateDecryptor()
                Dim plainTextBytes As Byte() = decryptor.TransformFinalBlock(cipherTextBytes, 0, cipherTextBytes.Length)
                Return Encoding.UTF8.GetString(plainTextBytes)
            End Using
        End Using
    End Function
End Class

