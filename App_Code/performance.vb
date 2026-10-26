Imports Microsoft.VisualBasic
Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Net.Mail
Imports System.Resources

Public Class performance
    Inherits DB_Interface
    Dim ds As New ds_functions
    Public Sub New()

        Non_Query("CREATE TABLE If NOT Exists `pergroups` (`grpname` varchar(100) DEFAULT NULL, `low` int(11) DEFAULT NULL, `high` int(11) DEFAULT NULL, `frequency` varchar(100) DEFAULT NULL," & _
       "`id` int(11) NOT NULL AUTO_INCREMENT," & _
"PRIMARY KEY (`id`) USING BTREE" & _
     ") ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1")

        Non_Query("CREATE TABLE If NOT Exists `indicators` (`pergrp` int(11) DEFAULT NULL, `indicator` varchar(100) DEFAULT NULL, `used` tinyint(4) NOT NULL DEFAULT '1', " & _
       "`id` int(11) NOT NULL AUTO_INCREMENT," & _
"PRIMARY KEY (`id`) USING BTREE" & _
     ") ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1")

        Non_Query("CREATE TABLE If NOT Exists `termind` (" & _
  "`session` int(11) NOT NULL," & _
  "`student` varchar(100) NOT NULL," & _
  "`indicator` int(11) DEFAULT NULL," & _
  "`period` varchar(100) NOT NULL," & _
  "`value` varchar(10) NOT NULL," & _
  "`id` int(11) NOT NULL AUTO_INCREMENT," & _
  "PRIMARY KEY (`id`) USING BTREE" & _
") ENGINE=InnoDB AUTO_INCREMENT=1227 DEFAULT CHARSET=latin1")


        Dim scon As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
        Non_Query("SELECT count(*) " & _
    "INTO @exist " & _
    "FROM information_schema.columns " & _
    "WHERE table_schema = '" & scon.Database & "' " & _
    "and COLUMN_NAME = 'pergrp ' " & _
    "AND table_name = 'class' LIMIT 1;" & _
    "set @query = IF(@exist <= 0, 'ALTER TABLE " & scon.Database & ".`class` ADD COLUMN `pergrp` int(11) NOT NULL', " & _
     "'select \'Column Exists\' status');" & _
    "prepare stmt from @query;" & _
    "EXECUTE stmt;")
    End Sub
End Class
