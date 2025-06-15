use recipe_share;

CREATE TABLE `rgh_user`
(
    `usrId`                     BIGINT              NOT NULL AUTO_INCREMENT,
    `usrGuid`                   BINARY(16)          UNIQUE NOT NULL,
    `usrName`                   VARCHAR(255)        NOT NULL,
    `usrEmail`                  VARCHAR(125)        UNIQUE NOT NULL,
    `usrPassword`               blob                NOT NULL,
    `usrLastLogin`              DATETIME            DEFAULT NULL,
    `usrIsActive`               BIT(1)              DEFAULT b'1' NOT NULL ,
    `usrCreatedBy`              BIGINT              NOT NULL,
    `usrCreatedByName`          VARCHAR(100)        NOT NULL,
    `usrCreatedOn`              DATETIME            DEFAULT CURRENT_TIMESTAMP NOT NULL,
    `usrUpdatedBy`              BIGINT              NOT NULL,
    `usrUpdatedByName`          VARCHAR(100)        NOT NULL,
    `usrUpdatedOn`              DATETIME            DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP NOT NULL,
    PRIMARY KEY (`usrId`)
);

CREATE TABLE `rgh_right`
(
    `rghId`                     BIGINT              NOT NULL,
    `rghParentId`               BIGINT              DEFAULT NULL,
    `rghCode`                   VARCHAR(255)        NOT NULL,
    `rghName`                   VARCHAR(255)        NOT NULL,
    `rghURL`                    TEXT,
    `rghIsMenu`                 BIT(1)              DEFAULT b'0' NOT NULL ,
    `rghType`                   TINYINT             NOT NULL,
    `rghIsActive`               BIT(1)              DEFAULT b'1' NOT NULL ,
    `rghCreatedBy`              BIGINT              NOT NULL,
    `rghCreatedByName`          VARCHAR(100)        NOT NULL,
    `rghCreatedOn`              DATETIME            DEFAULT CURRENT_TIMESTAMP NOT NULL,
    `rghUpdatedBy`              BIGINT              NOT NULL,
    `rghUpdatedByName`          VARCHAR(100)        NOT NULL,
    `rghUpdatedOn`              DATETIME            DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP NOT NULL,
    PRIMARY KEY (`rghId`),
    KEY `fk_right_parent_idx` (`rghParentId`),
    KEY `fk_right_created_by_idx` (`rghCreatedBy`),
    KEY `fk_right_updated_by_idx` (`rghUpdatedBy`),
    CONSTRAINT `fk_right_created_by` FOREIGN KEY (`rghCreatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_right_parent` FOREIGN KEY (`rghParentId`) REFERENCES `rgh_right` (`rghId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_right_updated_by` FOREIGN KEY (`rghUpdatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `rgh_user_right`
(
    `urgId`                     BIGINT              NOT NULL AUTO_INCREMENT,
    `urgUserId`                 BIGINT              NOT NULL,
    `urgRightId`                BIGINT              NOT NULL,
    `urgIsActive`               BIT(1)              NOT NULL DEFAULT b'1',
    `urgCreatedBy`              BIGINT              NOT NULL,
    `urgCreatedByName`          VARCHAR(100)        NOT NULL,
    `urgCreatedOn`              DATETIME            DEFAULT CURRENT_TIMESTAMP NOT NULL,
    `urgUpdatedBy`              BIGINT              NOT NULL,
    `urgUpdatedByName`          VARCHAR(100)        NOT NULL,
    `urgUpdatedOn`              DATETIME            DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP NOT NULL,
    PRIMARY KEY (`urgId`),
    KEY `fk_user_right_created_by_idx` (`urgCreatedBy`),
    KEY `fk_user_right_updated_by_idx` (`urgUpdatedBy`),
    KEY `fk_user_right_right_idx` (`urgRightId`),
    KEY `fk_user_right_user_idx` (`urgUserId`),
    CONSTRAINT `fk_user_right_created_by` FOREIGN KEY (`urgCreatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_user_right_right` FOREIGN KEY (`urgRightId`) REFERENCES `rgh_right` (`rghId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_user_right_updated_by` FOREIGN KEY (`urgUpdatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_user_right_user` FOREIGN KEY (`urgUserId`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `rgh_user_token`
(
    `utoId`                     BIGINT              NOT NULL AUTO_INCREMENT,
    `utoGuid`                   BINARY(16)          NOT NULL,
    `utoUserId`                 BIGINT              NOT NULL,
    `utoExpirationDate`         DATETIME            NOT NULL,
    `utoToken`                  TEXT                NOT NULL,
    `utoUserAgent`              TEXT                NOT NULL,
    `utoIpAddress`              TEXT                NOT NULL,
    `utoIsActive`               TINYINT             NOT NULL DEFAULT '1',
    `utoCreatedOn`              DATETIME            NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `utoUpdatedOn`              DATETIME            NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`utoId`),
    KEY `fk_user_token_user_idx` (`utoUserId`),
    KEY `inx_is_active_created` (`utoIsActive`, `utoCreatedOn`),
    CONSTRAINT `fk_user_token_user` FOREIGN KEY (`utoUserId`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION
);