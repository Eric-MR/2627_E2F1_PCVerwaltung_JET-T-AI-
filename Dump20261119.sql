CREATE DATABASE  IF NOT EXISTS `pc_verwaltung` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `pc_verwaltung`;
-- MySQL dump 10.13  Distrib 8.0.45, for Win64 (x86_64)
--
-- Host: localhost    Database: pc_verwaltung
-- ------------------------------------------------------
-- Server version	5.5.5-10.4.32-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `adresse`
--

DROP TABLE IF EXISTS `adresse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `adresse` (
  `adresse_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `kunde_id` bigint(20) NOT NULL,
  `strasse` varchar(150) NOT NULL,
  `hausnummer` varchar(20) NOT NULL,
  `plz` varchar(10) NOT NULL,
  `ort` varchar(100) NOT NULL,
  `land` varchar(100) NOT NULL,
  `adress_typ` varchar(50) NOT NULL,
  PRIMARY KEY (`adresse_id`),
  KEY `fk_adresse_kunde` (`kunde_id`),
  CONSTRAINT `fk_adresse_kunde` FOREIGN KEY (`kunde_id`) REFERENCES `kunde` (`kunde_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `adresse`
--

LOCK TABLES `adresse` WRITE;
/*!40000 ALTER TABLE `adresse` DISABLE KEYS */;
/*!40000 ALTER TABLE `adresse` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `benutzer`
--

DROP TABLE IF EXISTS `benutzer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `benutzer` (
  `benutzer_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `username` varchar(100) NOT NULL,
  `passwort_hash` varchar(255) NOT NULL,
  `email` varchar(255) DEFAULT NULL,
  `aktiv` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`benutzer_id`),
  UNIQUE KEY `username` (`username`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `benutzer`
--

LOCK TABLES `benutzer` WRITE;
/*!40000 ALTER TABLE `benutzer` DISABLE KEYS */;
/*!40000 ALTER TABLE `benutzer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `benutzer_rolle`
--

DROP TABLE IF EXISTS `benutzer_rolle`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `benutzer_rolle` (
  `benutzer_id` bigint(20) NOT NULL,
  `rolle_id` bigint(20) NOT NULL,
  PRIMARY KEY (`benutzer_id`,`rolle_id`),
  KEY `fk_benutzer_rolle_rolle` (`rolle_id`),
  CONSTRAINT `fk_benutzer_rolle_benutzer` FOREIGN KEY (`benutzer_id`) REFERENCES `benutzer` (`benutzer_id`) ON DELETE CASCADE,
  CONSTRAINT `fk_benutzer_rolle_rolle` FOREIGN KEY (`rolle_id`) REFERENCES `rolle` (`rolle_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `benutzer_rolle`
--

LOCK TABLES `benutzer_rolle` WRITE;
/*!40000 ALTER TABLE `benutzer_rolle` DISABLE KEYS */;
/*!40000 ALTER TABLE `benutzer_rolle` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `finanzierung`
--

DROP TABLE IF EXISTS `finanzierung`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `finanzierung` (
  `finanzierung_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `rechnung_id` bigint(20) NOT NULL,
  `anzahl_raten` int(11) NOT NULL,
  `zinssatz` decimal(5,2) NOT NULL,
  `laufzeit_monate` int(11) NOT NULL,
  `ratenbetrag` decimal(10,2) NOT NULL,
  `gesamtbetrag` decimal(10,2) NOT NULL,
  PRIMARY KEY (`finanzierung_id`),
  UNIQUE KEY `rechnung_id` (`rechnung_id`),
  CONSTRAINT `fk_finanzierung_rechnung` FOREIGN KEY (`rechnung_id`) REFERENCES `rechnung` (`rechnung_id`) ON DELETE CASCADE,
  CONSTRAINT `chk_finanzierung_raten` CHECK (`anzahl_raten` > 0),
  CONSTRAINT `chk_finanzierung_laufzeit` CHECK (`laufzeit_monate` > 0),
  CONSTRAINT `chk_finanzierung_zinssatz` CHECK (`zinssatz` >= 0),
  CONSTRAINT `chk_finanzierung_ratenbetrag` CHECK (`ratenbetrag` >= 0),
  CONSTRAINT `chk_finanzierung_gesamtbetrag` CHECK (`gesamtbetrag` >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `finanzierung`
--

LOCK TABLES `finanzierung` WRITE;
/*!40000 ALTER TABLE `finanzierung` DISABLE KEYS */;
/*!40000 ALTER TABLE `finanzierung` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hardwarekomponente`
--

DROP TABLE IF EXISTS `hardwarekomponente`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hardwarekomponente` (
  `komponente_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `komponententyp_id` bigint(20) NOT NULL,
  `hersteller` varchar(100) NOT NULL,
  `modell` varchar(150) NOT NULL,
  `seriennummer` varchar(150) DEFAULT NULL,
  `beschreibung` text DEFAULT NULL,
  `preis` decimal(10,2) NOT NULL,
  `lagerbestand` int(11) NOT NULL DEFAULT 0,
  `aktiv` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`komponente_id`),
  KEY `fk_hardware_komponententyp` (`komponententyp_id`),
  CONSTRAINT `fk_hardware_komponententyp` FOREIGN KEY (`komponententyp_id`) REFERENCES `komponententyp` (`komponententyp_id`),
  CONSTRAINT `chk_hardware_preis` CHECK (`preis` >= 0),
  CONSTRAINT `chk_hardware_lagerbestand` CHECK (`lagerbestand` >= 0)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hardwarekomponente`
--

LOCK TABLES `hardwarekomponente` WRITE;
/*!40000 ALTER TABLE `hardwarekomponente` DISABLE KEYS */;
INSERT INTO `hardwarekomponente` VALUES (1,1,'Fractal Design','Meshify 2','','Formfaktor: ATX',0.00,0,1),(2,1,'Cooler Master','NR400','','Formfaktor: MicroATX',0.00,0,1),(3,1,'NZXT','H1','','Formfaktor: MiniITX',0.00,0,1),(4,1,'be quiet!','Pure Base 500DX','','Formfaktor: ATX',0.00,0,1),(5,1,'Lian Li','O11 Dynamic','','Formfaktor: ATX',0.00,0,1),(6,2,'AMD','Ryzen 7 7800X3D','','Takt: 4.2 GHz',0.00,0,1),(7,2,'Intel','Core i5-13600K','','Takt: 3.5 GHz',0.00,0,1),(8,2,'AMD','Ryzen 5 5600','','Takt: 3.5 GHz',0.00,0,1),(9,2,'Intel','Core i7-12700F','','Takt: 2.1 GHz',0.00,0,1),(10,2,'AMD','Ryzen 7 5700G','','Takt: 3.8 GHz',0.00,0,1),(11,3,'ASUS','TUF GAMING B650-PLUS','','Formfaktor: ATX; Sockel: AM5',189.99,4,1),(12,3,'MSI','PRO B760M-A','','Formfaktor: MicroATX; Sockel: LGA1700',0.00,0,1),(13,3,'Gigabyte','B550I AORUS PRO AX','','Formfaktor: MiniITX; Sockel: AM4',0.00,0,1),(14,3,'ASRock','B650M Pro RS','','Formfaktor: MicroATX; Sockel: AM5',0.00,0,1),(15,3,'ASUS','ROG Strix Z690-A','','Formfaktor: ATX; Sockel: LGA1700',0.00,0,1);
/*!40000 ALTER TABLE `hardwarekomponente` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ip_adresse`
--

DROP TABLE IF EXISTS `ip_adresse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ip_adresse` (
  `ip_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `netzwerk_id` bigint(20) NOT NULL,
  `pc_id` bigint(20) NOT NULL,
  `ip_adresse` varchar(45) NOT NULL,
  PRIMARY KEY (`ip_id`),
  UNIQUE KEY `pc_id` (`pc_id`),
  UNIQUE KEY `ip_adresse` (`ip_adresse`),
  KEY `fk_ip_netzwerk` (`netzwerk_id`),
  CONSTRAINT `fk_ip_netzwerk` FOREIGN KEY (`netzwerk_id`) REFERENCES `netzwerk` (`netzwerk_id`),
  CONSTRAINT `fk_ip_pc` FOREIGN KEY (`pc_id`) REFERENCES `pc_system` (`pc_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ip_adresse`
--

LOCK TABLES `ip_adresse` WRITE;
/*!40000 ALTER TABLE `ip_adresse` DISABLE KEYS */;
/*!40000 ALTER TABLE `ip_adresse` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `komponententyp`
--

DROP TABLE IF EXISTS `komponententyp`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `komponententyp` (
  `komponententyp_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `bezeichnung` varchar(100) NOT NULL,
  PRIMARY KEY (`komponententyp_id`),
  UNIQUE KEY `bezeichnung` (`bezeichnung`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `komponententyp`
--

LOCK TABLES `komponententyp` WRITE;
/*!40000 ALTER TABLE `komponententyp` DISABLE KEYS */;
INSERT INTO `komponententyp` VALUES (1,'case'),(2,'cpu'),(3,'mainboard');
/*!40000 ALTER TABLE `komponententyp` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kunde`
--

DROP TABLE IF EXISTS `kunde`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kunde` (
  `kunde_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `kundennummer` varchar(50) NOT NULL,
  `vorname` varchar(100) NOT NULL,
  `nachname` varchar(100) NOT NULL,
  `email` varchar(255) DEFAULT NULL,
  `telefon` varchar(50) DEFAULT NULL,
  `ranking` enum('PRIME','STANDARD','LOW') NOT NULL,
  `zahlungsart_id` bigint(20) DEFAULT NULL,
  `erstellt_am` datetime NOT NULL DEFAULT current_timestamp(),
  `aktiv` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`kunde_id`),
  UNIQUE KEY `kundennummer` (`kundennummer`),
  KEY `fk_kunde_zahlungsart` (`zahlungsart_id`),
  CONSTRAINT `fk_kunde_zahlungsart` FOREIGN KEY (`zahlungsart_id`) REFERENCES `zahlungsart` (`zahlungsart_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kunde`
--

LOCK TABLES `kunde` WRITE;
/*!40000 ALTER TABLE `kunde` DISABLE KEYS */;
/*!40000 ALTER TABLE `kunde` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `netzwerk`
--

DROP TABLE IF EXISTS `netzwerk`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `netzwerk` (
  `netzwerk_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `bezeichnung` varchar(100) NOT NULL,
  `netzwerkadresse` varchar(45) NOT NULL,
  `subnetzmaske` varchar(45) NOT NULL,
  `gateway` varchar(45) NOT NULL,
  PRIMARY KEY (`netzwerk_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `netzwerk`
--

LOCK TABLES `netzwerk` WRITE;
/*!40000 ALTER TABLE `netzwerk` DISABLE KEYS */;
/*!40000 ALTER TABLE `netzwerk` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pc_system`
--

DROP TABLE IF EXISTS `pc_system`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pc_system` (
  `pc_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `kunde_id` bigint(20) NOT NULL,
  `bezeichnung` varchar(150) NOT NULL,
  `status` varchar(50) NOT NULL,
  `erstellt_am` datetime NOT NULL DEFAULT current_timestamp(),
  `gesamtpreis` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`pc_id`),
  KEY `fk_pc_kunde` (`kunde_id`),
  CONSTRAINT `fk_pc_kunde` FOREIGN KEY (`kunde_id`) REFERENCES `kunde` (`kunde_id`),
  CONSTRAINT `chk_pc_gesamtpreis` CHECK (`gesamtpreis` is null or `gesamtpreis` >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pc_system`
--

LOCK TABLES `pc_system` WRITE;
/*!40000 ALTER TABLE `pc_system` DISABLE KEYS */;
/*!40000 ALTER TABLE `pc_system` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pc_system_komponente`
--

DROP TABLE IF EXISTS `pc_system_komponente`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pc_system_komponente` (
  `pc_id` bigint(20) NOT NULL,
  `komponente_id` bigint(20) NOT NULL,
  `anzahl` int(11) NOT NULL DEFAULT 1,
  `preis_zum_zeitpunkt` decimal(10,2) NOT NULL,
  PRIMARY KEY (`pc_id`,`komponente_id`),
  KEY `fk_pc_komponente_hardware` (`komponente_id`),
  CONSTRAINT `fk_pc_komponente_hardware` FOREIGN KEY (`komponente_id`) REFERENCES `hardwarekomponente` (`komponente_id`),
  CONSTRAINT `fk_pc_komponente_pc` FOREIGN KEY (`pc_id`) REFERENCES `pc_system` (`pc_id`) ON DELETE CASCADE,
  CONSTRAINT `chk_pc_komponente_anzahl` CHECK (`anzahl` > 0),
  CONSTRAINT `chk_pc_komponente_preis` CHECK (`preis_zum_zeitpunkt` >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pc_system_komponente`
--

LOCK TABLES `pc_system_komponente` WRITE;
/*!40000 ALTER TABLE `pc_system_komponente` DISABLE KEYS */;
/*!40000 ALTER TABLE `pc_system_komponente` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rabattregel`
--

DROP TABLE IF EXISTS `rabattregel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rabattregel` (
  `rabattregel_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ranking` enum('PRIME','STANDARD','LOW') NOT NULL,
  `rabatt_prozent` decimal(5,2) NOT NULL,
  `gueltig_ab` date NOT NULL,
  PRIMARY KEY (`rabattregel_id`),
  UNIQUE KEY `ranking` (`ranking`),
  CONSTRAINT `chk_rabatt_prozent` CHECK (`rabatt_prozent` >= 0 and `rabatt_prozent` <= 100)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rabattregel`
--

LOCK TABLES `rabattregel` WRITE;
/*!40000 ALTER TABLE `rabattregel` DISABLE KEYS */;
/*!40000 ALTER TABLE `rabattregel` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rechnung`
--

DROP TABLE IF EXISTS `rechnung`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rechnung` (
  `rechnung_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `rechnungsnummer` varchar(50) NOT NULL,
  `kunde_id` bigint(20) NOT NULL,
  `rechnungsdatum` date NOT NULL,
  `zahlungsart_id` bigint(20) NOT NULL,
  `status` varchar(50) NOT NULL,
  `netto_betrag` decimal(10,2) NOT NULL,
  `steuer_betrag` decimal(10,2) NOT NULL,
  `brutto_betrag` decimal(10,2) NOT NULL,
  `rabatt_prozent` decimal(5,2) NOT NULL DEFAULT 0.00,
  `rabatt_betrag` decimal(10,2) NOT NULL DEFAULT 0.00,
  `gesamtbetrag` decimal(10,2) NOT NULL,
  PRIMARY KEY (`rechnung_id`),
  UNIQUE KEY `rechnungsnummer` (`rechnungsnummer`),
  KEY `fk_rechnung_kunde` (`kunde_id`),
  KEY `fk_rechnung_zahlungsart` (`zahlungsart_id`),
  CONSTRAINT `fk_rechnung_kunde` FOREIGN KEY (`kunde_id`) REFERENCES `kunde` (`kunde_id`),
  CONSTRAINT `fk_rechnung_zahlungsart` FOREIGN KEY (`zahlungsart_id`) REFERENCES `zahlungsart` (`zahlungsart_id`),
  CONSTRAINT `chk_rechnung_rabatt` CHECK (`rabatt_prozent` >= 0 and `rabatt_prozent` <= 100),
  CONSTRAINT `chk_rechnung_betraege` CHECK (`netto_betrag` >= 0 and `steuer_betrag` >= 0 and `brutto_betrag` >= 0 and `rabatt_betrag` >= 0 and `gesamtbetrag` >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rechnung`
--

LOCK TABLES `rechnung` WRITE;
/*!40000 ALTER TABLE `rechnung` DISABLE KEYS */;
/*!40000 ALTER TABLE `rechnung` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rechnungsposition`
--

DROP TABLE IF EXISTS `rechnungsposition`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rechnungsposition` (
  `rechnungsposition_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `rechnung_id` bigint(20) NOT NULL,
  `beschreibung` varchar(255) NOT NULL,
  `menge` int(11) NOT NULL,
  `einzelpreis` decimal(10,2) NOT NULL,
  `steuersatz` decimal(5,2) NOT NULL,
  `gesamtpreis` decimal(10,2) NOT NULL,
  PRIMARY KEY (`rechnungsposition_id`),
  KEY `fk_rechnungsposition_rechnung` (`rechnung_id`),
  CONSTRAINT `fk_rechnungsposition_rechnung` FOREIGN KEY (`rechnung_id`) REFERENCES `rechnung` (`rechnung_id`) ON DELETE CASCADE,
  CONSTRAINT `chk_rechnungsposition_menge` CHECK (`menge` > 0),
  CONSTRAINT `chk_rechnungsposition_einzelpreis` CHECK (`einzelpreis` >= 0),
  CONSTRAINT `chk_rechnungsposition_steuersatz` CHECK (`steuersatz` >= 0 and `steuersatz` <= 100),
  CONSTRAINT `chk_rechnungsposition_gesamtpreis` CHECK (`gesamtpreis` >= 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rechnungsposition`
--

LOCK TABLES `rechnungsposition` WRITE;
/*!40000 ALTER TABLE `rechnungsposition` DISABLE KEYS */;
/*!40000 ALTER TABLE `rechnungsposition` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rolle`
--

DROP TABLE IF EXISTS `rolle`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rolle` (
  `rolle_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `bezeichnung` varchar(100) NOT NULL,
  PRIMARY KEY (`rolle_id`),
  UNIQUE KEY `bezeichnung` (`bezeichnung`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rolle`
--

LOCK TABLES `rolle` WRITE;
/*!40000 ALTER TABLE `rolle` DISABLE KEYS */;
/*!40000 ALTER TABLE `rolle` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `zahlungsart`
--

DROP TABLE IF EXISTS `zahlungsart`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `zahlungsart` (
  `zahlungsart_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `bezeichnung` varchar(100) NOT NULL,
  PRIMARY KEY (`zahlungsart_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `zahlungsart`
--

LOCK TABLES `zahlungsart` WRITE;
/*!40000 ALTER TABLE `zahlungsart` DISABLE KEYS */;
/*!40000 ALTER TABLE `zahlungsart` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'pc_verwaltung'
--

--
-- Dumping routines for database 'pc_verwaltung'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-11-19 10:08:48
