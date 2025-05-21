-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1
-- Généré le : mer. 21 mai 2025 à 14:35
-- Version du serveur : 10.4.32-MariaDB
-- Version de PHP : 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `examdotnet`
--

-- --------------------------------------------------------

--
-- Structure de la table `commandeproduits`
--

CREATE TABLE `commandeproduits` (
  `IdCommande` int(11) NOT NULL,
  `IdProduit` int(11) NOT NULL,
  `IdCommandeProduit` int(11) NOT NULL,
  `Quantite` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `commandes`
--

CREATE TABLE `commandes` (
  `IdCommande` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `DateCommande` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `grandcategories`
--

CREATE TABLE `grandcategories` (
  `IdGrandCategorie` int(11) NOT NULL,
  `NomGrandCategorie` varchar(50) NOT NULL,
  `ImagePath` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `grandcategories`
--

INSERT INTO `grandcategories` (`IdGrandCategorie`, `NomGrandCategorie`, `ImagePath`) VALUES
(2, 'Instrument de Music', '/images/categories/40f0a950-2574-4903-8d65-fc7518ca8107_79767_1.jpg'),
(3, 'Sonorisation', '/images/categories/6bce577a-8fb9-4149-a540-3e9afbbdaa12_65304_1.jpg'),
(4, 'Câblerie', '/images/categories/6f0a3806-c942-488c-9c3d-107d4e3db14d_47929_1.jpg'),
(5, 'Flight Case', '/images/categories/3d66ae3b-f79c-4dea-9134-13b66ebd5fc0_66343_1.jpg'),
(6, 'Eclairage', '/images/categories/1756e178-5589-44eb-8770-f2b847eac485_80975_1 (1).jpg'),
(7, 'TV & Video', '/images/categories/111b6f35-c96d-476c-aa1e-5770a8802597_98519_1.jpg');

-- --------------------------------------------------------

--
-- Structure de la table `produits`
--

CREATE TABLE `produits` (
  `IdProduit` int(11) NOT NULL,
  `NomProduit` varchar(50) NOT NULL,
  `Prix` decimal(65,30) NOT NULL,
  `Description` varchar(225) NOT NULL,
  `NbStock` int(11) NOT NULL,
  `Slug` longtext DEFAULT NULL,
  `ImagePath` longtext DEFAULT NULL,
  `IdSousCat` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `produits`
--

INSERT INTO `produits` (`IdProduit`, `NomProduit`, `Prix`, `Description`, `NbStock`, `Slug`, `ImagePath`, `IdSousCat`) VALUES
(1, 'Korg - Pa5X 88', 4498.000000000000000000000000000000, 'Le Korg Pa5X 88 est un clavier arrangeur professionnel de 88 touches mcanisme marteau, offrant une vaste palette sonore avec plus de 2000 sons et 600 styles. Son boîtier robuste en aluminium avec flancs en bois, ses options d', 5, 'korg-pa5x-88', '/images/products/ab540a94-9536-4368-805c-fb283dae4942_83877_1.jpg', 35),
(2, 'Nord - Stage 4 88', 4359.000000000000000000000000000000, 'Le Nord Stage 4 88 propose 88 touches lestes, des sections piano, synth et orgue enrichies, et une section d’effets ddie. Sa mmoire double et ses fonctionnalits avances comme l’arpgiateur et la compression sidechain offrent u', 0, 'nord-stage-4-88', '/images/products/bbe8798c-f990-4e20-8362-e6a82c412860_88578_1.jpg', 35),
(3, 'Mapex - Venus 22\" 5F Crimson Red', 689.000000000000000000000000000000, 'Le kit de batterie Mapex Venus 22\" 5F Crimson Red est parfait pour les batteurs dbutants. Avec des fts en peuplier, des bords de roulement SONIClear™ pour un accordage facile, des cymbales en laiton martel et un matriel de qu', 5, 'mapex-venus-22-5f-crimson-red', '/images/products/a48ebc06-6464-4c51-928d-020f62f0b8a8_91060_1.jpg', 36),
(4, 'Gibson - SG Standard Ebony', 1499.000000000000000000000000000000, 'La SG est la guitare lectrique Gibson USA la plus vendue depuis sa naissance en 1961 ! Lgre, efficace, au sustain ingalable... Bref, pas besoin d\'en rajouter, La SG Standard ne change rien, pour rester une valeur sre, et perp', 8, 'gibson-sg-standard-ebony', '/images/products/758922ab-5652-4e36-8d3e-8e415fb09571_67120_1.jpg', 37),
(5, 'Yamaha - CG142S', 349.000000000000000000000000000000, 'Guitare Classique, Table en épicéa, Dos, éclisses et manche Nato', 5, 'yamaha-cg142s', '/images/products/f29b0880-fe09-4d57-b563-1aff22a4f4f2_23843_1.jpg', 38),
(6, 'Ibanez - SR300EDX', 469.000000000000000000000000000000, 'La basse Ibanez SR300EDX-WZM se distingue par son allure audacieuse avec son coloris Wine Red Frozen Matte. Associ cette finition captivante, son manche en rable/noyer 5 pices garantit une clart sonore exceptionnelle et un je', 0, 'ibanez-sr300edx', '/images/products/7005eb2a-7859-4752-aa60-8df87d156fd4_101618_1.jpg', 39),
(7, 'Mac Mah - AS 115', 399.000000000000000000000000000000, 'Enceinte active Mac Mah AS 115, 15\" 1000 W bi-amplifiée avec amplificateur numérique et DSP, une enceinte compacte idéale pour DJ.', 3, 'mac-mah-as-115', '/images/products/e0a25a9d-27f8-4d66-9967-eef69c9a7e9a_70913_1.jpg', 40),
(8, 'FBT - X-Lite 112A', 519.000000000000000000000000000000, 'Enceinte amplifiée, 2 voies Bass-reflex, amplification numérique 1500W, avec mixer 3 canaux, connectivité Bluetooth, DSP 4 Preset, haut parleur de 12\" custom, dans un boitier en polypropylène.', 2, 'fbt-x-lite-112a', '/images/products/e6b30613-c1fc-42e7-b077-998c8c6fa4fb_78556_1.jpg', 40),
(9, 'HPA - Promix 10', 349.000000000000000000000000000000, 'La HPA Promix 10 est une console de mixage analogique 10 canaux dote de multi-effets et d\'une connectivit USB 2.0. Elle offre huit entres micro, six entres ligne et deux entres stro, avec une alimentation fantme de +48 V. Ada', 7, 'hpa-promix-10', '/images/products/70ee1d82-35c6-43e6-84ef-4631ef60b212_89719_1.jpg', 42),
(10, 'Yamaha - MG16XU', 549.000000000000000000000000000000, 'La Yamaha MG16XU est une console de mixage analogique 16 canaux avec multi-effets SPX intgr, 10 entres micros avec pramplis classe A, EQ 3 bandes, pad attnuateur de 26dB et interface audio USB 2 entres/2 sorties avec une rsol', 5, 'yamaha-mg16xu', '/images/products/92049ace-67a9-4cb5-9652-ef0458cfceab_42297_1.jpg', 42),
(11, 'Zoom - LiveTrak L-20', 899.000000000000000000000000000000, 'La Zoom LiveTrak L-20 est une console de mixage numrique 20 canaux avec 16 entres mono, 2 entres stro, et la capacit d\'enregistrer simultanment 20 pistes. Elle propose une interface audio USB 22x4, des effets intgrs, 6 sortie', 6, 'zoom-livetrak-l-20', '/images/products/8e3a6564-762c-4d95-bec9-69c3bd58d349_63921_1.jpg', 43),
(12, 'Allen & Heath - CQ-20B', 869.000000000000000000000000000000, 'La gamme CQ d\'Allen & Heath redfinit la portabilit des tables de mixage numriques. Le modle CQ-20B est compact, mais puissant, avec 16 pramplis micro/ligne et de nombreuses options de connectivit, offrant une solution idale p', 9, 'allen-heath-cq-20b', '/images/products/dad4963c-43bd-45ed-93db-873dec032cf6_91027_1.jpg', 43),
(13, 'B&C Speakers - 8 PE 21', 109.000000000000000000000000000000, 'Haut Parleur 8\" (21cm) 400W AES / 8 ohms Aimant Ferrite B&C 8PE21', 10, 'bc-speakers-8-pe-21', '/images/products/42c21da5-e9e0-4fd4-8c55-d7a358c68c82_8864_1.jpg', 44),
(14, 'Eminence - BETA 8A', 75.000000000000000000000000000000, 'Haut Parleur 8\" 225W RMS / 8 Ohms Basse-Medium EMINENCE BETA 8A, HP 20 cm', 7, 'eminence-beta-8a', '/images/products/62d48c4f-dae5-4a2e-800c-475386434a83_4341_1.jpg', 44),
(15, 'Plugger - Câble XLR mâle 3b', 3.900000000000000000000000000000, 'Câble XLR mâle 3 broches / Jack mâle Stéréo 6,35 mm, section 2x 1,5 mm², diamètre OD6, longueur 0,6 mètre Plugger', 6, 'plugger-câble-xlr-mâle-3b', '/images/products/ce1867f6-7b1e-4756-9522-b8ea4500df34_47938_1.jpg', 46),
(16, 'Plugger - Câble XLR femelle', 10.000000000000000000000000000000, 'Câble XLR femelle / Jack mâle stéréo 6,35 mm, diamètre OD6, longueur 6 mètres Plugger.', 10, 'plugger-câble-xlr-femelle', '/images/products/d479ac39-8c93-4e02-aad2-56b41048ae48_47922_1.jpg', 46),
(17, 'Plugger - Câble Tweed Jack Mâle Mono', 8.900000000000000000000000000000, 'Câble Tweed Jack Mâle Mono 6.35mm / Jack Mâle Mono 6.35mm Coudé, longueur 3 mètres PLUGGER', 13, 'plugger-câble-tweed-jack-mâle-mono', '/images/products/06c06f38-f2cf-421a-8106-0e816ac8e994_47949_1.jpg', 47),
(18, 'Klotz - Cable LaGrange Jack', 59.000000000000000000000000000000, 'Klotz LA-GPP0300, cble guitare, de 3 mtres, quip de 2 jacks Ø6,35 mm droits 2 contacts chacun. Le cble LaGrange Supreme offre une rponse rapide, une super dynamique et une compression extrmement faible. Cela signifie que le s', 6, 'klotz-cable-lagrange-jack', '/images/products/98c0ba40-ef3c-4a59-b7a9-904ceb311dee_92566_1.jpg', 47),
(19, 'Evolite - Evo Spot 120 flight case 2in1', 259.000000000000000000000000000000, 'Flight case pour evolite Evo Spot 120, idéal pour transporter vos lyres evolite en toute sécurité.', 3, 'evolite-evo-spot-120-flight-case-2in1', '/images/products/dbbd465e-445e-4b3d-bafc-a555bf78c082_66343_1 (1).jpg', 50),
(20, 'Chauvet - CHS 40', 27.000000000000000000000000000000, 'Sac de transport Chauvet CHS 40, idéal pour transporter votre jeu de lumière Chauvet comme le Double Derby, Swarm...', 15, 'chauvet-chs-40', '/images/products/7385408f-1494-414d-bba5-2ecf4c9e3c02_40580_1.jpg', 50),
(21, 'Dap - Universal Foam Case 2', 159.000000000000000000000000000000, 'Universal Foam Case 2', 0, 'dap-universal-foam-case-2', '/images/products/1760a264-cf10-4fda-b08a-14cf7fee4da7_30392_1.jpg', 49),
(22, 'Walkasse - Flight Clavier 88', 299.000000000000000000000000000000, 'Protgez efficacement votre clavier 88 touches avec le Flight Case Walkasse pour KONTROL S88. Conu en contreplaqu WBP de bouleau finlandais de 7 mm, il offre une finition extrieure mlamine rsistante aux chocs et rayures. L\'int', 12, 'walkasse-flight-clavier-88', '/images/products/4fdb8a88-1f57-44c3-b0de-e5be8fc4e974_100142_1 (1).jpg', 51),
(23, 'Walkasse - Flight Clavier 49', 219.000000000000000000000000000000, 'Le Walkasse Flight Clavier 49 Kontrol S49 est un flight case robuste spécialement conçu pour transporter et protéger les claviers 49 touches comme le Native Instruments KONTROL S49. Fabriqué en Betonex et contreplaqué WBP fin', 6, 'walkasse-flight-clavier-49', '/images/products/9646645a-04ed-431f-83c1-f344b2cfc2f2_100138_1.jpg', 51),
(24, 'Elokance - Pack e-Slim 110', 499.000000000000000000000000000000, 'Le systme d\'enceintes amplifies Elokance e-Slim vous offre une solution de sonorisation professionnelle complte et performante, accessible tous. Fournis ici avec son lot de housses, que vous soyez musicien, DJ, organisateur d', 10, 'elokance-pack-e-slim-110', '/images/products/2894dc9d-7d84-4c55-845a-a027f8a338e3_93630_1.jpg', 41),
(25, 'FBT - Pack X-PRO 115A (la paire)', 1590.000000000000000000000000000000, 'Pack de deux enceintes amplifies FBT X-Pro 115A + housses + pieds d\'enceintes SV-200 II fournis avec leur housse de transport. Les enceintes 2 voies FBT X-Pro 115A sont quipes de haut-parleur de 15\", aliment par des leur ampl', 15, 'fbt-pack-x-pro-115a-la-paire', '/images/products/5e63eb6d-c075-4f08-84cb-06c85c62af68_79177_1.jpg', 41),
(26, 'Mipro - ACT-32H', 149.000000000000000000000000000000, 'Emetteur main sans fil ACT-32H pour sono portable de marque Mipro.', 0, 'mipro-act-32h', '/images/products/99a65c02-3f5f-4a79-ab23-12a70d0765b1_45135_1.jpg', 45),
(27, 'Neutrik - NL4FXX-W-S', 5.900000000000000000000000000000, 'Connecteur SpeakON 4 pôles Neutrik NL4FXX-W-S pour câbles de 6 à 12 mm. Durée de vie de 5000 connexions avec verrouillage par torsion.', 25, 'neutrik-nl4fxx-w-s', '/images/products/1b180ad6-705c-4392-851b-e0636fcc72bc_89683_1.jpg', 48),
(28, 'Panasonic - PT-VMZ71BE', 4366.000000000000000000000000000000, 'Pour des prsentations qualitatives mme dans des endroits trs lumineux, Panasonic vous propose le projecteur laser LCD PT-VMZ71BE Black. Ce modle est compact et lger d\'environ 7 kg pour plus de flexibilit lors de l\'installatio', 4, 'panasonic-pt-vmz71be', '/images/products/fc55b12f-db55-4d5f-ad81-e9ce006f8fdd_98736_1.jpg', 52),
(29, 'Panasonic - PT-REQ12BE Black', 27364.000000000000000000000000000000, 'La PT-REQ12BE, un projecteur laser de qualité supérieure de couleur noire signé Panasonic, est réputée pour sa fiabilité, sa polyvalence et son ergonomie. Ce projecteur, pourvu de plusieurs technologies de la marque comme le ', 8, 'panasonic-pt-req12be-black', '/images/products/1fe384ba-b2f5-4a51-b4c3-1a8840fd9656_102726_1.jpg', 52),
(30, 'Panasonic - TH-55CQE2-IR 55\'\' 4K Tactile', 3466.000000000000000000000000000000, 'Panasonic propose l\'cran TH-55CQE2-IR 55\'\' 4K Tactile, pour une utilisation polyvalente, que ce soit en runion d\'entreprise ou dans le domaine ducative. Il dispose d\'un cran LCD tactile avec une haute luminosit et un bon cont', 12, 'panasonic-th-55cqe2-ir-55-4k-tactile', '/images/products/524ae802-68af-4510-9b3c-0b9cf332ff5e_103099_1.jpg', 53);

-- --------------------------------------------------------

--
-- Structure de la table `souscategories`
--

CREATE TABLE `souscategories` (
  `IdSousCategorie` int(11) NOT NULL,
  `NomSousCategorie` varchar(50) NOT NULL,
  `ImagePath` longtext DEFAULT NULL,
  `IdGrandCat` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `souscategories`
--

INSERT INTO `souscategories` (`IdSousCategorie`, `NomSousCategorie`, `ImagePath`, `IdGrandCat`) VALUES
(35, 'Clavier & Piano', '/images/souscategories/d1d83b1c-5e24-4b6f-936c-96a4132fcd20_90358_1.jpg', 2),
(36, 'Batterie', '/images/souscategories/ace9287e-825b-4c87-b1cb-077763709fec_55083_1.jpg', 2),
(37, 'Guitare Electrique', '/images/souscategories/57c5d44f-05c5-43c0-bf30-438190eb480e_97298_1.jpg', 2),
(38, 'Guitare Accoustique', '/images/souscategories/c0fba7d4-d317-4599-af5a-3252e0205f04_64473_1.jpg', 2),
(39, 'Guitare Bass', '/images/souscategories/c3e74be5-69bf-4ac5-ae75-fad714628d47_101619_1.jpg', 2),
(40, 'Enceinte Amplifiée', '/images/souscategories/86cf3378-0b0b-4c74-bbf4-55e09d32e5ad_65304_1.jpg', 3),
(41, 'Pack Enceinte', '/images/souscategories/5cebef0e-49c7-4d31-84b1-2c55f22dcf44_93629_1.jpg', 3),
(42, 'Console de Mixage Analogique', '/images/souscategories/7c4fe9b5-bab4-4102-acf9-996195b79e26_89721_1.jpg', 3),
(43, 'Console de Mixage Numérique', '/images/souscategories/26d64130-1a4b-49a6-a528-5c6eb186b462_66685_1.jpg', 3),
(44, 'Haut Parleur', '/images/souscategories/5eb3b4b9-e4a9-4d8a-9e1f-dc3bf25d300b_8885_1.jpg', 3),
(45, 'Micro', '/images/souscategories/3b25c6ce-cfd7-41d3-bf74-0b07459e8a99_87939_1.jpg', 3),
(46, 'Cable Audio', '/images/souscategories/e87c5176-cb76-4b4a-a314-bf2ac5cca01a_47929_1.jpg', 4),
(47, 'Cable Instrument', '/images/souscategories/54dcb766-2669-4e60-9e32-cf3e9a55e0ca_47959_1.jpg', 4),
(48, 'Adapteur', '/images/souscategories/a8ede57b-3749-407a-bd37-a4f785432388_90996_1.jpg', 4),
(49, 'Flight Case Utilitaire', '/images/souscategories/a3c2a9a2-7c80-4297-8633-050550e249e7_75068_1.jpg', 5),
(50, 'Flight Case Eclairage', '/images/souscategories/aa1f437a-7840-452f-ac57-40b0981be10b_75064_1.jpg', 5),
(51, 'Flight Case Instrument', '/images/souscategories/45d47c52-0a4c-421b-9aa9-29c15c1bfaad_100142_1.jpg', 5),
(52, 'VideoProjecteur', NULL, 7),
(53, 'TV Ultra HD 4K', '/images/souscategories/68873957-e588-445c-b4a9-25dfd8863007_98885_1.jpg', 7);

-- --------------------------------------------------------

--
-- Structure de la table `users`
--

CREATE TABLE `users` (
  `Id` int(11) NOT NULL,
  `Email` varchar(200) NOT NULL,
  `Password` varchar(100) NOT NULL,
  `Genre` longtext NOT NULL,
  `Nom` varchar(100) NOT NULL,
  `Prenom` varchar(100) NOT NULL,
  `DateNaissance` datetime(6) DEFAULT NULL,
  `Telephone` longtext NOT NULL,
  `Adresse` longtext NOT NULL,
  `Ville` longtext NOT NULL,
  `Pays` longtext NOT NULL,
  `Role` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `users`
--

INSERT INTO `users` (`Id`, `Email`, `Password`, `Genre`, `Nom`, `Prenom`, `DateNaissance`, `Telephone`, `Adresse`, `Ville`, `Pays`, `Role`) VALUES
(1, 'harena@gmail.com', '$2a$11$WEZ8e4z1N1VzRZnqca1J1uGDo6taOtM0MmrJ.TCGUZvX/LFYdZfZC', 'Mr', 'Misandratra', 'Harena', '2010-12-21 00:00:00.000000', '0346214484', 'Lot IVJ 01 Ambohimiadana Avaratra', 'Antananarivo', 'Madagascar', 1),
(2, 'sarobidyharena21@gmail.com', '$2a$11$VIwIO6gqDS4/YUskLikBye9NEYobTFG7WHS0lN48lTeJlvDHpptDq', 'Mr', 'Rakotoarimanana', 'Sarobidy', '2006-06-26 00:00:00.000000', '0346122130', 'Lot II K 27 Près Ambodivona Soavimasoandro', 'Antananarivo', 'Madagascar', 0);

-- --------------------------------------------------------

--
-- Structure de la table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20250507114821_DatabaseUpdate', '8.0.8');

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `commandeproduits`
--
ALTER TABLE `commandeproduits`
  ADD PRIMARY KEY (`IdCommande`,`IdProduit`),
  ADD KEY `IX_CommandeProduits_IdProduit` (`IdProduit`);

--
-- Index pour la table `commandes`
--
ALTER TABLE `commandes`
  ADD PRIMARY KEY (`IdCommande`),
  ADD KEY `IX_Commandes_UserId` (`UserId`);

--
-- Index pour la table `grandcategories`
--
ALTER TABLE `grandcategories`
  ADD PRIMARY KEY (`IdGrandCategorie`);

--
-- Index pour la table `produits`
--
ALTER TABLE `produits`
  ADD PRIMARY KEY (`IdProduit`),
  ADD KEY `IX_Produits_IdSousCat` (`IdSousCat`);

--
-- Index pour la table `souscategories`
--
ALTER TABLE `souscategories`
  ADD PRIMARY KEY (`IdSousCategorie`),
  ADD KEY `IX_SousCategories_IdGrandCat` (`IdGrandCat`);

--
-- Index pour la table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`);

--
-- Index pour la table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `commandes`
--
ALTER TABLE `commandes`
  MODIFY `IdCommande` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `grandcategories`
--
ALTER TABLE `grandcategories`
  MODIFY `IdGrandCategorie` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT pour la table `produits`
--
ALTER TABLE `produits`
  MODIFY `IdProduit` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT pour la table `souscategories`
--
ALTER TABLE `souscategories`
  MODIFY `IdSousCategorie` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=54;

--
-- AUTO_INCREMENT pour la table `users`
--
ALTER TABLE `users`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `commandeproduits`
--
ALTER TABLE `commandeproduits`
  ADD CONSTRAINT `FK_CommandeProduits_Commandes_IdCommande` FOREIGN KEY (`IdCommande`) REFERENCES `commandes` (`IdCommande`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_CommandeProduits_Produits_IdProduit` FOREIGN KEY (`IdProduit`) REFERENCES `produits` (`IdProduit`) ON DELETE CASCADE;

--
-- Contraintes pour la table `commandes`
--
ALTER TABLE `commandes`
  ADD CONSTRAINT `FK_Commandes_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Contraintes pour la table `produits`
--
ALTER TABLE `produits`
  ADD CONSTRAINT `FK_Produits_SousCategories_IdSousCat` FOREIGN KEY (`IdSousCat`) REFERENCES `souscategories` (`IdSousCategorie`) ON DELETE CASCADE;

--
-- Contraintes pour la table `souscategories`
--
ALTER TABLE `souscategories`
  ADD CONSTRAINT `FK_SousCategories_GrandCategories_IdGrandCat` FOREIGN KEY (`IdGrandCat`) REFERENCES `grandcategories` (`IdGrandCategorie`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
