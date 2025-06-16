use recipe_share;


CREATE TABLE `trx_recipe`
(
    `rcpId`                     BIGINT              NOT NULL AUTO_INCREMENT,
    `rcpGuid`                   BINARY(16)          UNIQUE NOT NULL,
    `rcpName`                   VARCHAR(255)        NOT NULL,
    `rcpCookingTimeMinutes`     SMALLINT            NOT NULL,
    `rcpIsActive`               TINYINT             NOT NULL DEFAULT '1',
    `rcpCreatedBy`              BIGINT              NOT NULL,
    `rcpCreatedByName`          VARCHAR(100)        NOT NULL,
    `rcpCreatedOn`              DATETIME            DEFAULT CURRENT_TIMESTAMP NOT NULL,
    `rcpUpdatedBy`              BIGINT              NOT NULL,
    `rcpUpdatedByName`          VARCHAR(100)        NOT NULL,
    `rcpUpdatedOn`              DATETIME            DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP NOT NULL,
    PRIMARY KEY (`rcpId`),
    KEY `fk_trx_recipe_created_by_idx` (`rcpCreatedBy`),
    KEY `fk_trx_recipe_updated_by_idx` (`rcpUpdatedBy`),
    CONSTRAINT `fk_trx_recipe_created_by` FOREIGN KEY (`rcpCreatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_trx_recipe_updated_by` FOREIGN KEY (`rcpUpdatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `mtn_ingredient`
(
    `ingId`                     BIGINT              NOT NULL AUTO_INCREMENT,
    `ingGuid`                   BINARY(16)          UNIQUE NOT NULL,
    `ingName`                   VARCHAR(255)        UNIQUE NOT NULL,
    `ingIsActive`               TINYINT             NOT NULL DEFAULT '1',
    `ingCreatedBy`              BIGINT              NOT NULL,
    `ingCreatedByName`          VARCHAR(100)        NOT NULL,
    `ingUpdatedBy`              BIGINT              NOT NULL,
    `ingUpdatedByName`          VARCHAR(100)        NOT NULL,
    `ingCreatedOn`              DATETIME            NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `ingUpdatedOn`              DATETIME            NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`ingId`),
    KEY `fk_mtn_ingredient_created_by_idx` (`ingCreatedBy`),
    KEY `fk_mtn_ingredient_updated_by_idx` (`ingUpdatedBy`),
    CONSTRAINT `fk_mtn_ingredient_created_by` FOREIGN KEY (`ingCreatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_mtn_ingredient_updated_by` FOREIGN KEY (`ingUpdatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `mtn_recipe_ingredient`
(
    `rpiId`             BIGINT                      NOT NULL AUTO_INCREMENT,
    `rpiRecipeId`       BIGINT                      NOT NULL,
    `rpiIngredientId`   BIGINT                      NOT NULL,
    `rpiQuantity`       SMALLINT                    NOT NULL,
    `rpiIsActive`       BIT(1)                      NOT NULL DEFAULT b'1',
    `rpiCreatedBy`      BIGINT                      NOT NULL,
    `rpiCreatedByName`  VARCHAR(100)                NOT NULL,
    `rpiCreatedOn`      DATETIME                    DEFAULT CURRENT_TIMESTAMP NOT NULL,
    `rpiUpdatedBy`      BIGINT                      NOT NULL,
    `rpiUpdatedByName`  VARCHAR(100)                NOT NULL,
    `rpiUpdatedOn`      DATETIME                    DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP NOT NULL,
    PRIMARY KEY (`rpiId`),
    UNIQUE KEY `mtn_recipe_ingredient_recipe_ingredient_unq_k` (`rpiRecipeId`, `rpiIngredientId`),
    KEY `fk_mtn_recipe_ingredient_recipe_idx` (`rpiRecipeId`),
    KEY `fk_mtn_recipe_ingredient_ingredient_idx` (`rpiIngredientId`),
    KEY `fk_mtn_recipe_ingredient_created_by_idx` (`rpiCreatedBy`),
    KEY `fk_mtn_recipe_ingredient_updated_by_idx` (`rpiUpdatedBy`),
    CONSTRAINT `fk_mtn_recipe_ingredient_recipe` FOREIGN KEY (`rpiRecipeId`) REFERENCES `trx_recipe` (`rcpId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_mtn_recipe_ingredient_ingredient` FOREIGN KEY (`rpiIngredientId`) REFERENCES `mtn_ingredient` (`ingId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_mtn_recipe_ingredient_created_by` FOREIGN KEY (`rpiCreatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_mtn_recipe_ingredient_updated_by` FOREIGN KEY (`rpiUpdatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `mtn_step`
(
    `stpId`                     BIGINT              NOT NULL AUTO_INCREMENT,
    `stpGuid`                   BINARY(16)          UNIQUE NOT NULL,
    `stpIndex`                  SMALLINT            NOT NULL,
    `stpName`                   VARCHAR(255)        NOT NULL,
    `stpRecipeId`               BIGINT              NOT NULL,
    `stpIsActive`               TINYINT             NOT NULL DEFAULT '1',
    `stpCreatedBy`              BIGINT              NOT NULL,
    `stpCreatedByName`          VARCHAR(100)        NOT NULL,
    `stpCreatedOn`              DATETIME            DEFAULT CURRENT_TIMESTAMP NOT NULL,
    `stpUpdatedBy`              BIGINT              NOT NULL,
    `stpUpdatedByName`          VARCHAR(100)        NOT NULL,
    `stpUpdatedOn`              DATETIME            DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP NOT NULL,
    PRIMARY KEY (`stpId`),
    KEY `fk_mtn_step_recipe_idx` (`stpRecipeId`),
    KEY `fk_mtn_step_created_by_idx` (`stpCreatedBy`),
    KEY `fk_mtn_step_updated_by_idx` (`stpUpdatedBy`),
    CONSTRAINT `fk_mtn_step_recipe` FOREIGN KEY (`stpRecipeId`) REFERENCES `trx_recipe` (`rcpId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_mtn_step_created_by` FOREIGN KEY (`stpCreatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_mtn_step_updated_by` FOREIGN KEY (`stpUpdatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `mtn_dietary_tag`
(
    `dtgId`                     BIGINT              NOT NULL AUTO_INCREMENT,
    `dtgGuid`                   BINARY(16)          UNIQUE NOT NULL,
    `dtgName`                   VARCHAR(255)        NOT NULL,
    `dtgIsActive`               TINYINT             NOT NULL DEFAULT '1',
    `dtgCreatedBy`              BIGINT              NOT NULL,
    `dtgCreatedByName`          VARCHAR(100)        NOT NULL,
    `dtgCreatedOn`              DATETIME            DEFAULT CURRENT_TIMESTAMP NOT NULL,
    `dtgUpdatedBy`              BIGINT              NOT NULL,
    `dtgUpdatedByName`          VARCHAR(100)        NOT NULL,
    `dtgUpdatedOn`              DATETIME            DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP NOT NULL,
    PRIMARY KEY (`dtgId`),
    KEY `fk_mtn_dietary_tag_created_by_idx` (`dtgCreatedBy`),
    KEY `fk_mtn_dietary_tag_updated_by_idx` (`dtgUpdatedBy`),
    CONSTRAINT `fk_mtn_dietary_tag_created_by` FOREIGN KEY (`dtgCreatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_mtn_dietary_tag_updated_by` FOREIGN KEY (`dtgUpdatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE `mtn_recipe_dietary_tag`
(
    `rdtId`                     BIGINT              NOT NULL AUTO_INCREMENT,
    `rdtRecipeId`               BIGINT              NOT NULL,
    `rdtDietaryTagId`           BIGINT              NOT NULL,
    `rdtIsActive`               BIT(1)              NOT NULL DEFAULT b'1',
    `rdtCreatedBy`              BIGINT              NOT NULL,
    `rdtCreatedByName`          VARCHAR(100)        NOT NULL,
    `rdtCreatedOn`              DATETIME            DEFAULT CURRENT_TIMESTAMP NOT NULL,
    `rdtUpdatedBy`              BIGINT              NOT NULL,
    `rdtUpdatedByName`          VARCHAR(100)        NOT NULL,
    `rdtUpdatedOn`              DATETIME            DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP NOT NULL,
    PRIMARY KEY (`rdtId`),
    UNIQUE KEY `mtn_recipe_dietary_tag_recipe_dietary_tag_unq_k` (`rdtRecipeId`, `rdtDietaryTagId`),
    KEY `fk_mtn_recipe_dietary_tag_recipe_idx` (`rdtRecipeId`),
    KEY `fk_mtn_recipe_dietary_tag_dietary_tag_idx` (`rdtDietaryTagId`),
    KEY `fk_mtn_recipe_dietary_tag_created_by_idx` (`rdtCreatedBy`),
    KEY `fk_mtn_recipe_dietary_tag_updated_by_idx` (`rdtUpdatedBy`),
    CONSTRAINT `fk_mtn_recipe_dietary_tag_recipe` FOREIGN KEY (`rdtRecipeId`) REFERENCES `trx_recipe` (`rcpId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_mtn_recipe_dietary_tag_dietary_tag` FOREIGN KEY (`rdtDietaryTagId`) REFERENCES `mtn_dietary_tag` (`dtgId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_mtn_recipe_dietary_tag_created_by` FOREIGN KEY (`rdtCreatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT `fk_mtn_recipe_dietary_tag_updated_by` FOREIGN KEY (`rdtUpdatedBy`) REFERENCES `rgh_user` (`usrId`) ON DELETE NO ACTION ON UPDATE NO ACTION
);