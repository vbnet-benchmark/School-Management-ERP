-- phpMyAdmin SQL Dump
-- version 3.5.2
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Sep 17, 2019 at 02:53 PM
-- Server version: 5.5.25a
-- PHP Version: 5.4.4

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `school-mgt-test`
--

-- --------------------------------------------------------

--
-- Table structure for table `acclogin`
--

CREATE TABLE IF NOT EXISTS `acclogin` (
  `username` varchar(100) DEFAULT NULL,
  `password` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `acclogin`
--



-- --------------------------------------------------------

--
-- Table structure for table `accounts`
--

CREATE TABLE IF NOT EXISTS `accounts` (
  `username` varchar(25) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `accsettings`
--

CREATE TABLE IF NOT EXISTS `accsettings` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `accname` varchar(255) DEFAULT NULL,
  `type` varchar(25) DEFAULT NULL,
  `initial` int(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=54 ;

--
-- Dumping data for table `accsettings`
--

INSERT INTO `accsettings` (`id`, `accname`, `type`, `initial`) VALUES
(2, 'BANK ACCOUNT', 'cash', 4350000),
(24, 'SALARY EXPENSES', 'expense', 0),
(25, 'BOARDING DEBTS', 'income', 0),
(26, 'BOARDING PAID', 'income', 0),
(27, 'TRANSPORT DBTS', 'income', 0),
(28, 'TRANSPORT PAID', 'income', 0),
(29, 'ALLOWANCES', 'expense', 0),
(30, 'SALARY DEDUCTIONS', 'income', 0),
(31, 'DEBTORS', 'receivable', 1340000),
(49, 'ADVANCE FEE PAYMENT', 'liability', 0);

-- --------------------------------------------------------

--
-- Table structure for table `admin`
--

CREATE TABLE IF NOT EXISTS `admin` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `password` varchar(255) DEFAULT NULL,
  `username` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=4 ;

--
-- Dumping data for table `admin`
--

INSERT INTO `admin` (`ID`, `password`, `username`) VALUES
(1, 'demo1', 'demo1');


-- --------------------------------------------------------

--
-- Table structure for table `assignments`
--

CREATE TABLE IF NOT EXISTS `assignments` (
  `Id` int(11) NOT NULL,
  `title` varchar(100) DEFAULT NULL,
  `session` int(11) DEFAULT NULL,
  `class` int(11) DEFAULT NULL,
  `date` datetime DEFAULT NULL,
  `deadline` varchar(100) DEFAULT NULL,
  `subject` int(11) DEFAULT NULL,
  `assignment` text,
  `teacher` varchar(25) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `assignments`
--


-- --------------------------------------------------------

--
-- Table structure for table `attachments`
--

CREATE TABLE IF NOT EXISTS `attachments` (
  `msgId` int(11) DEFAULT NULL,
  `file` varchar(255) DEFAULT NULL,
  `fileicon` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `attachments`
--


-- --------------------------------------------------------

--
-- Table structure for table `attendance`
--

CREATE TABLE IF NOT EXISTS `attendance` (
  `week` int(11) DEFAULT NULL,
  `student` varchar(25) DEFAULT NULL,
  `term` int(11) DEFAULT NULL,
  `date` varchar(50) NOT NULL,
  `morning` tinyint(1) DEFAULT NULL,
  `afternoon` tinyint(1) DEFAULT NULL,
  `remarks` varchar(100) DEFAULT NULL,
  `class` int(11) DEFAULT NULL,
  `holiday` tinyint(4) NOT NULL DEFAULT '0',
  `id` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=97 ;

--
-- Dumping data for table `attendance`
--


-- --------------------------------------------------------

--
-- Table structure for table `boarding`
--

CREATE TABLE IF NOT EXISTS `boarding` (
  `cost` varchar(100) NOT NULL,
  `account` varchar(100) NOT NULL,
  `min` int(11) NOT NULL DEFAULT '100'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `boarding`
--

INSERT INTO `boarding` (`cost`, `account`, `min`) VALUES
('18000', '', 100);

-- --------------------------------------------------------

--
-- Table structure for table `books`
--

CREATE TABLE IF NOT EXISTS `books` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `bookname` varchar(255) DEFAULT NULL,
  `author` varchar(100) DEFAULT NULL,
  `publisher` varchar(100) DEFAULT NULL,
  `subject` int(11) DEFAULT NULL,
  `file` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=14 ;

--
-- Dumping data for table `books`
--


-- --------------------------------------------------------

--
-- Table structure for table `choicefees`
--

CREATE TABLE IF NOT EXISTS `choicefees` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fee` varchar(100) DEFAULT NULL,
  `amount` int(11) DEFAULT NULL,
  `min` int(11) NOT NULL DEFAULT '100',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Dumping data for table `choicefees`
--

-- --------------------------------------------------------

--
-- Table structure for table `class`
--

CREATE TABLE IF NOT EXISTS `class` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Class` varchar(255) DEFAULT NULL,
  `studentNo` varchar(10) DEFAULT NULL,
  `superior` int(11) DEFAULT NULL,
  `type` varchar(255) DEFAULT NULL,
  `gradesystem` int(11) NOT NULL,
  `cano` int(11) DEFAULT NULL,
  `timetable` int(11) NOT NULL DEFAULT '0',
  `traitgroup` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=8 ;

--
-- Dumping data for table `class`
--

-- --------------------------------------------------------

--
-- Table structure for table `classfees`
--

CREATE TABLE IF NOT EXISTS `classfees` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `class` int(11) NOT NULL DEFAULT '0',
  `fee` varchar(30) DEFAULT NULL,
  `amount` varchar(20) DEFAULT NULL,
  `min` int(11) NOT NULL DEFAULT '100',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=7 ;

--
-- Dumping data for table `classfees`
--

-- --------------------------------------------------------

--
-- Table structure for table `classlevel`
--

CREATE TABLE IF NOT EXISTS `classlevel` (
  `ID` int(11) NOT NULL,
  `level` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `classonefees`
--

CREATE TABLE IF NOT EXISTS `classonefees` (
  `class` int(11) NOT NULL,
  `fee` varchar(100) DEFAULT NULL,
  `amount` varchar(20) DEFAULT NULL,
  `min` int(11) NOT NULL DEFAULT '100',
  `id` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Dumping data for table `classonefees`
--

-- --------------------------------------------------------

--
-- Table structure for table `classperiods`
--

CREATE TABLE IF NOT EXISTS `classperiods` (
  `period` int(11) DEFAULT NULL,
  `class` int(11) DEFAULT NULL,
  `activity` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `classperiods`
--


-- --------------------------------------------------------

--
-- Table structure for table `classsessionfees`
--

CREATE TABLE IF NOT EXISTS `classsessionfees` (
  `class` int(11) NOT NULL,
  `fee` varchar(100) DEFAULT NULL,
  `amount` varchar(20) DEFAULT NULL,
  `min` int(11) DEFAULT NULL,
  `id` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

--
-- Dumping data for table `classsessionfees`
--


-- --------------------------------------------------------

--
-- Table structure for table `classstats`
--

CREATE TABLE IF NOT EXISTS `classstats` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Session` int(11) DEFAULT NULL,
  `Class` int(11) DEFAULT NULL,
  `Parameter` varchar(255) DEFAULT NULL,
  `Number` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  KEY `FK_classstats_session` (`Session`),
  KEY `FK_classstats_class` (`Class`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `classsubjects`
--

CREATE TABLE IF NOT EXISTS `classsubjects` (
  `Class` int(11) DEFAULT NULL,
  `Subject` int(11) DEFAULT NULL,
  `periods` int(11) NOT NULL,
  `Type` varchar(255) DEFAULT NULL,
  `teacher` varchar(25) NOT NULL,
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sgroup` int(11) NOT NULL,
  `cgroup` int(11) NOT NULL,
  `doubles` int(11) NOT NULL,
  `subjectnest` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=387 ;

--
-- Dumping data for table `classsubjects`
--


-- --------------------------------------------------------

--
-- Table structure for table `classteacher`
--

CREATE TABLE IF NOT EXISTS `classteacher` (
  `teacher` varchar(25) DEFAULT NULL,
  `class` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `classteacher`
--



-- --------------------------------------------------------

--
-- Table structure for table `courseoutline`
--

CREATE TABLE IF NOT EXISTS `courseoutline` (
  `subject` int(11) DEFAULT NULL,
  `session` int(11) DEFAULT NULL,
  `class` int(11) DEFAULT NULL,
  `week` int(11) DEFAULT NULL,
  `topic` text,
  `content` text,
  `id` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=14 ;

--
-- Dumping data for table `courseoutline`
--


-- --------------------------------------------------------

--
-- Table structure for table `courseoverview`
--

CREATE TABLE IF NOT EXISTS `courseoverview` (
  `subject` int(11) DEFAULT NULL,
  `session` int(11) DEFAULT NULL,
  `class` int(11) DEFAULT NULL,
  `overview` text,
  `texts` varchar(255) NOT NULL,
  `classub` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `courseoverview`
--


-- --------------------------------------------------------

--
-- Table structure for table `depts`
--

CREATE TABLE IF NOT EXISTS `depts` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `dept` varchar(100) DEFAULT NULL,
  `head` varchar(25) NOT NULL,
  `superior` varchar(100) NOT NULL,
  `headtitle` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=6 ;

--
-- Dumping data for table `depts`
--


-- --------------------------------------------------------

--
-- Table structure for table `discount`
--

CREATE TABLE IF NOT EXISTS `discount` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `student` varchar(25) DEFAULT NULL,
  `fee` varchar(100) DEFAULT NULL,
  `type` varchar(25) DEFAULT NULL,
  `amount` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=16 ;

--
-- Dumping data for table `discount`
--


-- --------------------------------------------------------

--
-- Table structure for table `elearning`
--

CREATE TABLE IF NOT EXISTS `elearning` (
  `session` int(11) DEFAULT NULL,
  `subject` int(11) DEFAULT NULL,
  `class` int(11) DEFAULT NULL,
  `file` varchar(255) NOT NULL,
  `date` datetime DEFAULT NULL,
  `id` int(11) DEFAULT NULL,
  `teacher` varchar(100) DEFAULT NULL,
  `type` varchar(100) NOT NULL,
  `title` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE IF NOT EXISTS `error` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `time` datetime DEFAULT NULL,
  `object` varchar(100) DEFAULT NULL,
  `message` text,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=4438 ;

--
-- Dumping data for table `error`
--


-- --------------------------------------------------------

--
-- Table structure for table `expense`
--

CREATE TABLE IF NOT EXISTS `expense` (
  `ref` varchar(10) NOT NULL,
  `type` varchar(25) DEFAULT NULL,
  `amount` varchar(20) DEFAULT NULL,
  `remarks` varchar(255) DEFAULT NULL,
  `date` varchar(25) DEFAULT NULL,
  PRIMARY KEY (`ref`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `feeding`
--

CREATE TABLE IF NOT EXISTS `feeding` (
  `cost` varchar(25) NOT NULL,
  `account` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `feeschedule`
--

CREATE TABLE IF NOT EXISTS `feeschedule` (
  `session` int(11) DEFAULT NULL,
  `fee` varchar(100) DEFAULT NULL,
  `amount` int(20) DEFAULT NULL,
  `student` varchar(25) DEFAULT NULL,
  `class` int(11) DEFAULT NULL,
  `compulsory` tinyint(4) NOT NULL,
  `paid` int(20) NOT NULL DEFAULT '0',
  `min` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `feeschedule`
--


-- --------------------------------------------------------

--
-- Table structure for table `grades`
--

CREATE TABLE IF NOT EXISTS `grades` (
  `system` int(11) DEFAULT NULL,
  `lowest` int(11) DEFAULT NULL,
  `grade` varchar(10) DEFAULT NULL,
  `subject` varchar(100) DEFAULT NULL,
  `average` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `grades`
--


-- --------------------------------------------------------

--
-- Table structure for table `gradingsystem`
--

CREATE TABLE IF NOT EXISTS `gradingsystem` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `system` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Dumping data for table `gradingsystem`
--


-- --------------------------------------------------------

--
-- Table structure for table `hostel`
--

CREATE TABLE IF NOT EXISTS `hostel` (
  `hostel` varchar(100) DEFAULT NULL,
  `ward` varchar(25) DEFAULT NULL,
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Dumping data for table `hostel`
--

-- --------------------------------------------------------

--
-- Table structure for table `income`
--

CREATE TABLE IF NOT EXISTS `income` (
  `ref` varchar(10) NOT NULL DEFAULT '',
  `type` varchar(25) DEFAULT NULL,
  `amount` int(11) DEFAULT NULL,
  `remarks` varchar(255) DEFAULT NULL,
  `date` varchar(25) DEFAULT NULL,
  PRIMARY KEY (`ref`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `kcourseoutline`
--

CREATE TABLE IF NOT EXISTS `kcourseoutline` (
  `subject` int(11) DEFAULT NULL,
  `class` int(11) DEFAULT NULL,
  `session` int(11) DEFAULT NULL,
  `week` int(11) DEFAULT NULL,
  `topic` text,
  `score` varchar(10) DEFAULT NULL,
  `grade` varchar(10) DEFAULT NULL,
  `remarks` varchar(100) DEFAULT NULL,
  `content` text,
  `id` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=15 ;

--
-- Dumping data for table `kcourseoutline`
--


-- --------------------------------------------------------

--
-- Table structure for table `kscoresheet`
--

CREATE TABLE IF NOT EXISTS `kscoresheet` (
  `topic` int(11) DEFAULT NULL,
  `session` int(11) DEFAULT NULL,
  `class` int(11) DEFAULT NULL,
  `student` varchar(100) DEFAULT NULL,
  `grade` varchar(10) NOT NULL,
  `remarks` varchar(100) NOT NULL,
  `subject` int(11) DEFAULT NULL,
  `score` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `kscoresheet`
--


-- --------------------------------------------------------

--
-- Table structure for table `lessonplan`
--

CREATE TABLE IF NOT EXISTS `lessonplan` (
  `id` varchar(25) DEFAULT NULL,
  `teacher` varchar(25) DEFAULT NULL,
  `head` varchar(25) DEFAULT NULL,
  `subject` int(11) DEFAULT NULL,
  `class` int(11) DEFAULT NULL,
  `week` varchar(25) DEFAULT NULL,
  `plan` text,
  `date` datetime DEFAULT NULL,
  `session` int(11) DEFAULT NULL,
  `status` varchar(25) NOT NULL DEFAULT 'Unmarked',
  `remarks` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `lessonplan`
--


-- --------------------------------------------------------

--
-- Table structure for table `lib`
--

CREATE TABLE IF NOT EXISTS `lib` (
  `username` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `lib`
--


-- --------------------------------------------------------

--
-- Table structure for table `log`
--

CREATE TABLE IF NOT EXISTS `log` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `time` datetime DEFAULT NULL,
  `activity` text,
  `user` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1540 ;

--
-- Dumping data for table `log`
--


-- --------------------------------------------------------

--
-- Table structure for table `messages`
--

CREATE TABLE IF NOT EXISTS `messages` (
  `Id` int(11) NOT NULL,
  `sender` varchar(25) DEFAULT NULL,
  `receiver` varchar(25) DEFAULT NULL,
  `subject` varchar(255) DEFAULT NULL,
  `message` longtext,
  `date` datetime DEFAULT NULL,
  `session` varchar(10) DEFAULT NULL,
  `sendertype` varchar(100) DEFAULT NULL,
  `receivertype` varchar(100) DEFAULT NULL,
  `status` varchar(10) NOT NULL DEFAULT 'Unread'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `messages`
--


-- --------------------------------------------------------

--
-- Table structure for table `notifications`
--

CREATE TABLE IF NOT EXISTS `notifications` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `recipient` varchar(59) DEFAULT NULL,
  `message` text,
  `status` varchar(10) NOT NULL DEFAULT 'Unread',
  `origin` varchar(59) DEFAULT NULL,
  `type` varchar(100) DEFAULT NULL,
  `relationship` varchar(50) DEFAULT NULL,
  `time` datetime DEFAULT NULL,
  `url` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3528 ;

--
-- Dumping data for table `notifications`
--


-- --------------------------------------------------------

--
-- Table structure for table `onetimefees`
--

CREATE TABLE IF NOT EXISTS `onetimefees` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `fee` varchar(25) DEFAULT NULL,
  `amount` varchar(25) DEFAULT NULL,
  `min` int(11) NOT NULL DEFAULT '100',
  `account` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

--
-- Dumping data for table `onetimefees`
--

-- --------------------------------------------------------

--
-- Table structure for table `optionalfees`
--

CREATE TABLE IF NOT EXISTS `optionalfees` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `fee` varchar(25) DEFAULT NULL,
  `amount` varchar(25) DEFAULT NULL,
  `min` int(11) NOT NULL DEFAULT '100',
  `account` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `options`
--

CREATE TABLE IF NOT EXISTS `options` (
  `boarding` tinyint(1) NOT NULL,
  `transport` tinyint(1) NOT NULL,
  `fees` varchar(100) DEFAULT 'automatic',
  `email` varchar(100) DEFAULT NULL,
  `password` varchar(100) DEFAULT NULL,
  `port` varchar(10) DEFAULT NULL,
  `smsapi` varchar(255) DEFAULT NULL,
  `subacc` varchar(100) DEFAULT NULL,
  `pubkey` varchar(100) DEFAULT NULL,
  `signature` varchar(100) DEFAULT NULL,
  `smtp` varchar(100) DEFAULT NULL,
  `smsname` varchar(100) DEFAULT NULL,
  `logo` varchar(100) DEFAULT NULL,
  `seckey` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `options`
--

-- --------------------------------------------------------

--
-- Table structure for table `parentprofile`
--

CREATE TABLE IF NOT EXISTS `parentprofile` (
  `parentID` varchar(25) NOT NULL DEFAULT 'password',
  `parentName` varchar(100) DEFAULT NULL,
  `sex` varchar(10) DEFAULT NULL,
  `address` text,
  `email` varchar(30) DEFAULT NULL,
  `phone` varchar(19) DEFAULT NULL,
  `passport` varchar(100) DEFAULT NULL,
  `password` varchar(20) NOT NULL DEFAULT 'password',
  `activated` tinyint(1) NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `parentprofile`
--


-- --------------------------------------------------------

--
-- Table structure for table `parentward`
--

CREATE TABLE IF NOT EXISTS `parentward` (
  `parent` varchar(25) DEFAULT NULL,
  `ward` varchar(25) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `parentward`
--


-- --------------------------------------------------------

--
-- Table structure for table `pin`
--

CREATE TABLE IF NOT EXISTS `pin` (
  `ID` int(20) NOT NULL AUTO_INCREMENT,
  `admno` varchar(25) NOT NULL,
  `pin` int(11) DEFAULT NULL,
  `dateissued` varchar(255) DEFAULT NULL,
  `expiry` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `salschedule`
--

CREATE TABLE IF NOT EXISTS `salschedule` (
  `ref` varchar(20) NOT NULL,
  `Date` date NOT NULL,
  `Month` varchar(10) NOT NULL,
  `year` varchar(255) NOT NULL,
  `staffId` varchar(25) NOT NULL,
  `amount` int(20) NOT NULL,
  `tax` int(11) NOT NULL,
  `pension` int(11) NOT NULL,
  `bills` int(11) NOT NULL,
  `deductions` int(11) NOT NULL,
  `increments` int(11) NOT NULL,
  `net` int(11) NOT NULL,
  `status` tinyint(4) DEFAULT '0',
  `welfare` int(11) NOT NULL,
  `others` int(11) DEFAULT NULL,
  PRIMARY KEY (`ref`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `salschedule`
--


-- --------------------------------------------------------

--
-- Table structure for table `scorespublish`
--

CREATE TABLE IF NOT EXISTS `scorespublish` (
  `term` int(11) DEFAULT NULL,
  `CA1` tinyint(1) NOT NULL DEFAULT '0',
  `CA2` tinyint(1) NOT NULL DEFAULT '0',
  `CA3` tinyint(1) NOT NULL DEFAULT '0',
  `Project` tinyint(1) NOT NULL DEFAULT '0',
  `Exams` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `scorespublish`
--


-- --------------------------------------------------------

--
-- Table structure for table `sentmsgs`
--

CREATE TABLE IF NOT EXISTS `sentmsgs` (
  `id` int(11) DEFAULT NULL,
  `date` datetime DEFAULT NULL,
  `sender` varchar(50) DEFAULT NULL,
  `sendertype` varchar(50) DEFAULT NULL,
  `subject` varchar(100) DEFAULT NULL,
  `receiver` varchar(100) DEFAULT NULL,
  `message` text,
  `session` int(11) DEFAULT NULL,
  `receivertype` varchar(50) DEFAULT NULL,
  `receiverreply` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `sentmsgs`
--


-- --------------------------------------------------------

--
-- Table structure for table `session`
--

CREATE TABLE IF NOT EXISTS `session` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Session` varchar(255) DEFAULT NULL,
  `Term` varchar(255) DEFAULT NULL,
  `TotalNoTerms` int(11) DEFAULT NULL,
  `NextTerm` varchar(255) DEFAULT NULL,
  `ClosingDate` varchar(255) DEFAULT NULL,
  `Status` varchar(255) DEFAULT NULL,
  `opendate` varchar(255) DEFAULT NULL,
  `smsno` double NOT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=17 ;

--
-- Dumping data for table `session`
--


-- --------------------------------------------------------

--
-- Table structure for table `sessionalfees`
--

CREATE TABLE IF NOT EXISTS `sessionalfees` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `fee` varchar(25) DEFAULT NULL,
  `amount` varchar(25) DEFAULT NULL,
  `min` int(11) NOT NULL DEFAULT '100',
  `account` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `sessioncreate`
--

CREATE TABLE IF NOT EXISTS `sessioncreate` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `sessionname` varchar(20) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Dumping data for table `sessioncreate`
--


-- --------------------------------------------------------

--
-- Table structure for table `sms`
--

CREATE TABLE IF NOT EXISTS `sms` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `subject` varchar(255) DEFAULT NULL,
  `message` text,
  `units` int(11) DEFAULT NULL,
  `pages` int(11) DEFAULT NULL,
  `recipientsno` int(11) DEFAULT NULL,
  `recipienttype` varchar(100) DEFAULT NULL,
  `time` datetime DEFAULT NULL,
  `sender` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=7 ;

--
-- Dumping data for table `sms`
--


-- --------------------------------------------------------

--
-- Table structure for table `staffdept`
--

CREATE TABLE IF NOT EXISTS `staffdept` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `staff` varchar(100) DEFAULT NULL,
  `dept` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=30 ;

--
-- Dumping data for table `staffdept`
--


-- --------------------------------------------------------

--
-- Table structure for table `staffprofile`
--

CREATE TABLE IF NOT EXISTS `staffprofile` (
  `staffId` varchar(25) NOT NULL,
  `surname` varchar(100) DEFAULT NULL,
  `password` varchar(20) DEFAULT NULL,
  `sex` varchar(10) DEFAULT NULL,
  `phone` varchar(15) DEFAULT NULL,
  `dob` varchar(50) NOT NULL,
  `address` varchar(100) DEFAULT NULL,
  `email` varchar(30) NOT NULL,
  `passport` varchar(100) DEFAULT NULL,
  `Designation` varchar(255) NOT NULL,
  `salary` varchar(50) DEFAULT NULL,
  `activated` tinyint(1) NOT NULL DEFAULT '1',
  `pension` tinyint(1) NOT NULL DEFAULT '0',
  `accountno` bigint(200) DEFAULT NULL,
  `bank` varchar(50) DEFAULT NULL,
  `pfa` varchar(25) NOT NULL,
  PRIMARY KEY (`staffId`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `staffprofile`
--


-- --------------------------------------------------------

--
-- Table structure for table `stock`
--

CREATE TABLE IF NOT EXISTS `stock` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `item` varchar(100) DEFAULT NULL,
  `quantity` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=6 ;

--
-- Dumping data for table `stock`
--


-- --------------------------------------------------------

--
-- Table structure for table `stockadjust`
--

CREATE TABLE IF NOT EXISTS `stockadjust` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `date` datetime DEFAULT NULL,
  `item` int(11) DEFAULT NULL,
  `added` varchar(10) DEFAULT NULL,
  `removed` varchar(10) DEFAULT NULL,
  `details` text,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Dumping data for table `stockadjust`
--

-- --------------------------------------------------------

--
-- Table structure for table `studentfees`
--

CREATE TABLE IF NOT EXISTS `studentfees` (
  `student` varchar(25) NOT NULL,
  `session` int(11) DEFAULT NULL,
  `fee` varchar(25) DEFAULT NULL,
  `amount` varchar(25) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `studentsprofile`
--

CREATE TABLE IF NOT EXISTS `studentsprofile` (
  `admno` varchar(25) NOT NULL,
  `surname` varchar(255) DEFAULT NULL,
  `Sex` varchar(255) DEFAULT NULL,
  `dateOfBirth` varchar(25) DEFAULT NULL,
  `parent` varchar(255) DEFAULT NULL,
  `address` varchar(255) DEFAULT NULL,
  `phone` varchar(100) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `passport` varchar(100) DEFAULT NULL,
  `admfees` varchar(25) NOT NULL DEFAULT 'Not Paid',
  `discount` int(11) NOT NULL DEFAULT '0',
  `hostel` varchar(100) NOT NULL,
  `transport` varchar(100) NOT NULL,
  `hostelstay` tinyint(1) NOT NULL DEFAULT '0',
  `password` varchar(25) NOT NULL DEFAULT 'password',
  `activated` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`admno`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `studentsprofile`
--


-- --------------------------------------------------------

--
-- Table structure for table `studentsummary`
--

CREATE TABLE IF NOT EXISTS `studentsummary` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Session` int(11) DEFAULT NULL,
  `Class` int(11) DEFAULT NULL,
  `student` varchar(25) DEFAULT NULL,
  `Age` int(11) DEFAULT NULL,
  `Average` double NOT NULL,
  `Position` varchar(255) NOT NULL,
  `classTeacherRemarks` varchar(255) DEFAULT NULL,
  `principalRemarks` varchar(243) NOT NULL,
  `handWriting` varchar(255) NOT NULL,
  `fluency` varchar(255) NOT NULL,
  `games` varchar(255) NOT NULL,
  `sports` varchar(255) NOT NULL,
  `gymnastics` varchar(255) NOT NULL,
  `tools` varchar(255) NOT NULL,
  `drawing` varchar(255) NOT NULL,
  `crafts` varchar(255) NOT NULL,
  `Present` varchar(255) NOT NULL,
  `Absent` varchar(255) NOT NULL,
  `ClassNo` varchar(255) NOT NULL,
  `aveage` int(11) NOT NULL,
  `musical` varchar(10) NOT NULL,
  `punctual` varchar(10) NOT NULL,
  `attendance` varchar(10) NOT NULL,
  `reliability` varchar(10) NOT NULL,
  `neatness` varchar(10) NOT NULL,
  `polite` varchar(10) NOT NULL,
  `honesty` varchar(10) NOT NULL,
  `relate` varchar(10) NOT NULL,
  `selfcontrol` varchar(10) NOT NULL,
  `cooperation` varchar(10) NOT NULL,
  `responsibility` varchar(10) NOT NULL,
  `attentiveness` varchar(10) NOT NULL,
  `initiative` varchar(10) NOT NULL,
  `organization` varchar(10) NOT NULL,
  `perseverance` varchar(10) NOT NULL,
  `status` tinyint(1) NOT NULL DEFAULT '0',
  `trans` varchar(50) NOT NULL,
  `classhigh` varchar(10) DEFAULT NULL,
  `classlow` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=34959 ;

--
-- Dumping data for table `studentsummary`
--


-- --------------------------------------------------------

--
-- Table structure for table `subjectallocate`
--

CREATE TABLE IF NOT EXISTS `subjectallocate` (
  `class` int(11) DEFAULT NULL,
  `subject` int(11) DEFAULT NULL,
  `periods` int(11) DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `subjectnest`
--

CREATE TABLE IF NOT EXISTS `subjectnest` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=12 ;

--
-- Dumping data for table `subjectnest`
--

INSERT INTO `subjectnest` (`ID`, `name`) VALUES
(11, 'BASIC SCIENCE AND TECHNOLOGY');

-- --------------------------------------------------------

--
-- Table structure for table `subjectreg`
--

CREATE TABLE IF NOT EXISTS `subjectreg` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Session` int(11) DEFAULT NULL,
  `Class` int(11) DEFAULT NULL,
  `Student` varchar(25) DEFAULT NULL,
  `SubjectsOfferred` int(11) DEFAULT NULL,
  `CA1` varchar(11) NOT NULL,
  `CA2` varchar(11) NOT NULL,
  `CA3` varchar(11) NOT NULL,
  `Examination` varchar(11) NOT NULL,
  `Total` double NOT NULL,
  `Average` double NOT NULL,
  `Highest` double NOT NULL,
  `Lowest` double NOT NULL,
  `Grade` varchar(243) NOT NULL,
  `Remarks` varchar(243) NOT NULL,
  `pos` varchar(243) NOT NULL,
  `avg` double NOT NULL,
  `project` varchar(11) NOT NULL,
  `testtotal` double NOT NULL,
  `nested` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=36003 ;

--
-- Dumping data for table `subjectreg`
--


-- --------------------------------------------------------

--
-- Table structure for table `subjects`
--

CREATE TABLE IF NOT EXISTS `subjects` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Subject` varchar(255) DEFAULT NULL,
  `alias` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=17 ;

--
-- Dumping data for table `subjects`
--


-- --------------------------------------------------------

--
-- Table structure for table `subjectstats`
--

CREATE TABLE IF NOT EXISTS `subjectstats` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Session` int(11) DEFAULT NULL,
  `Class` int(11) DEFAULT NULL,
  `Subject` int(11) DEFAULT NULL,
  `Parameter` varchar(255) DEFAULT NULL,
  `Number` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  KEY `FK_subjectstats_session` (`Session`),
  KEY `FK_subjectstats_class` (`Class`),
  KEY `FK_subjectstats_subjects` (`Subject`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `subjectteacher`
--

CREATE TABLE IF NOT EXISTS `subjectteacher` (
  `teacher` varchar(25) DEFAULT NULL,
  `subject` int(11) DEFAULT NULL,
  `class` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `submittedassignments`
--

CREATE TABLE IF NOT EXISTS `submittedassignments` (
  `id` int(11) DEFAULT NULL,
  `student` varchar(25) DEFAULT NULL,
  `session` varchar(25) DEFAULT NULL,
  `class` int(11) DEFAULT NULL,
  `subject` int(11) DEFAULT NULL,
  `assignment` int(11) DEFAULT NULL,
  `answer` text,
  `date` datetime DEFAULT NULL,
  `remarks` text NOT NULL,
  `score` varchar(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `submittedassignments`
--


-- --------------------------------------------------------

--
-- Table structure for table `tcgroups`
--

CREATE TABLE IF NOT EXISTS `tcgroups` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `tclass`
--

CREATE TABLE IF NOT EXISTS `tclass` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Class` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `tday`
--

CREATE TABLE IF NOT EXISTS `tday` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Day` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `teacher`
--

CREATE TABLE IF NOT EXISTS `teacher` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Teacher` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `teacherperiods`
--

CREATE TABLE IF NOT EXISTS `teacherperiods` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Teacher` int(11) DEFAULT NULL,
  `Class` int(11) DEFAULT NULL,
  `Periods` int(11) DEFAULT NULL,
  `Subject` int(11) DEFAULT NULL,
  `Doubles` int(11) DEFAULT '0',
  `tname` varchar(15) NOT NULL,
  `Group` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  KEY `FK_teacherperiods_tclass_1` (`Class`),
  KEY `FK_teacherperiods_teacher` (`Teacher`),
  KEY `FK_teacherperiods_tsubject` (`Subject`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `termtraits`
--

CREATE TABLE IF NOT EXISTS `termtraits` (
  `session` int(11) NOT NULL,
  `student` varchar(100) NOT NULL,
  `trait` int(11) DEFAULT NULL,
  `value` varchar(1) NOT NULL DEFAULT '-',
  `id` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1128 ;

--
-- Dumping data for table `termtraits`
--


-- --------------------------------------------------------

--
-- Table structure for table `texemptions`
--

CREATE TABLE IF NOT EXISTS `texemptions` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `day` varchar(15) NOT NULL,
  `teacher` varchar(20) DEFAULT NULL,
  `timetable` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_texemptions_tday` (`day`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Dumping data for table `texemptions`
--


-- --------------------------------------------------------

--
-- Table structure for table `timetable`
--

CREATE TABLE IF NOT EXISTS `timetable` (
  `Day` varchar(18) DEFAULT NULL,
  `Period` int(11) DEFAULT NULL,
  `Subject` int(11) DEFAULT NULL,
  `Class` int(11) DEFAULT NULL,
  `Teacher` varchar(20) DEFAULT NULL,
  `Doubles` int(11) DEFAULT NULL,
  `tname` int(11) NOT NULL,
  `grouped` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `timetable`
--

-- --------------------------------------------------------

--
-- Table structure for table `tperiods`
--

CREATE TABLE IF NOT EXISTS `tperiods` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Day` varchar(10) DEFAULT NULL,
  `Period` int(11) DEFAULT NULL,
  `Timestart` time DEFAULT NULL,
  `timeend` time DEFAULT NULL,
  `activity` varchar(10) DEFAULT NULL,
  `timetable` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  KEY `FK_Table1_tday` (`Day`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=96 ;

--
-- Dumping data for table `tperiods`
--

-- --------------------------------------------------------

--
-- Table structure for table `traitgroup`
--

CREATE TABLE IF NOT EXISTS `traitgroup` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

--
-- Dumping data for table `traitgroup`
--


-- --------------------------------------------------------

--
-- Table structure for table `traits`
--

CREATE TABLE IF NOT EXISTS `traits` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `trait` varchar(40) DEFAULT NULL,
  `used` tinyint(4) NOT NULL DEFAULT '1',
  `traitgroup` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=9 ;

--
-- Dumping data for table `traits`
--


-- --------------------------------------------------------

--
-- Table structure for table `transactions`
--

CREATE TABLE IF NOT EXISTS `transactions` (
  `ref` varchar(20) NOT NULL DEFAULT '',
  `type` varchar(50) DEFAULT NULL,
  `dr` varchar(30) NOT NULL,
  `cr` varchar(30) NOT NULL,
  `account` varchar(255) DEFAULT NULL,
  `details` varchar(255) DEFAULT NULL,
  `date` datetime DEFAULT NULL,
  `trtype` varchar(10) DEFAULT NULL,
  `student` varchar(50) NOT NULL DEFAULT 'Nil',
  `session` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `transactions`
--


-- --------------------------------------------------------

--
-- Table structure for table `transportfees`
--

CREATE TABLE IF NOT EXISTS `transportfees` (
  `route` varchar(100) DEFAULT NULL,
  `amount` int(25) NOT NULL,
  `driver` varchar(25) DEFAULT NULL,
  `busno` varchar(10) DEFAULT NULL,
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `min` int(11) NOT NULL DEFAULT '100',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=7 ;

--
-- Dumping data for table `transportfees`
--

-- --------------------------------------------------------

--
-- Table structure for table `tsgroups`
--

CREATE TABLE IF NOT EXISTS `tsgroups` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_tsgroups_tclass_2` (`name`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=5 ;

--
-- Dumping data for table `tsgroups`
--


-- --------------------------------------------------------

--
-- Table structure for table `tsubject`
--

CREATE TABLE IF NOT EXISTS `tsubject` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Subject` int(11) NOT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Dumping data for table `tsubject`
--


-- --------------------------------------------------------

--
-- Table structure for table `ttname`
--

CREATE TABLE IF NOT EXISTS `ttname` (
  `name` varchar(100) DEFAULT NULL,
  `school` int(11) NOT NULL,
  `default` tinyint(4) NOT NULL DEFAULT '0',
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `class` int(11) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

--
-- Dumping data for table `ttname`
--


-- --------------------------------------------------------

--
-- Table structure for table `usersession`
--

CREATE TABLE IF NOT EXISTS `usersession` (
  `id` varchar(255) DEFAULT NULL,
  `user` varchar(100) DEFAULT NULL,
  `type` varchar(100) DEFAULT NULL,
  `active` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `teacherperiods`
--
ALTER TABLE `teacherperiods`
  ADD CONSTRAINT `FK_teacherperiods_tclass_1` FOREIGN KEY (`Class`) REFERENCES `tclass` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `FK_teacherperiods_teacher` FOREIGN KEY (`Teacher`) REFERENCES `teacher` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `FK_teacherperiods_tsubject` FOREIGN KEY (`Subject`) REFERENCES `tsubject` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `teacherperiods_ibfk_1` FOREIGN KEY (`Class`) REFERENCES `tclass` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `teacherperiods_ibfk_2` FOREIGN KEY (`Teacher`) REFERENCES `teacher` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `teacherperiods_ibfk_3` FOREIGN KEY (`Subject`) REFERENCES `tsubject` (`ID`) ON DELETE NO ACTION ON UPDATE NO ACTION;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
