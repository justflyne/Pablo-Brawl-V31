SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `brawldatabase`
--

-- --------------------------------------------------------

--
-- Table structure `accounts`
--

CREATE TABLE `accounts` (
  `Id` bigint(20) NOT NULL,
  `Trophies` int(11) NOT NULL,
  `Data` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL CHECK (json_valid(`Data`))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure `alliances`
--

CREATE TABLE `alliances` (
  `Id` bigint(20) NOT NULL,
  `Name` text NOT NULL,
  `Trophies` int(11) NOT NULL,
  `Data` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL CHECK (json_valid(`Data`))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure `users`
--

CREATE TABLE `users` (
  `password` varchar(60) NOT NULL,
  `id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure `telegram_bindings`
--

CREATE TABLE `telegram_bindings` (
  `telegram_user_id` bigint(20) NOT NULL,
  `account_id` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indexes
--

ALTER TABLE `accounts`
  ADD UNIQUE KEY `Id` (`Id`);

ALTER TABLE `alliances`
  ADD UNIQUE KEY `Id` (`Id`);

ALTER TABLE `users`
  ADD UNIQUE KEY `id` (`id`);

ALTER TABLE `telegram_bindings`
  ADD UNIQUE KEY `telegram_user_id` (`telegram_user_id`),
  ADD KEY `account_id` (`account_id`);

--
-- Foreign Keys
--

ALTER TABLE `telegram_bindings`
  ADD CONSTRAINT `telegram_bindings_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
