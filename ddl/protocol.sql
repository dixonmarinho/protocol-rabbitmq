/*
 Navicat Premium Data Transfer

 Source Server         : Postgres - Docker
 Source Server Type    : PostgreSQL
 Source Server Version : 130009 (130009)
 Source Host           : 10.0.0.11:5432
 Source Catalog        : protocol
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 130009 (130009)
 File Encoding         : 65001

 Date: 25/10/2024 15:51:09
*/


-- ----------------------------
-- Sequence structure for auth_role_claims_id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."auth_role_claims_id_seq";
CREATE SEQUENCE "public"."auth_role_claims_id_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for auth_user_claims_id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."auth_user_claims_id_seq";
CREATE SEQUENCE "public"."auth_user_claims_id_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1;

-- ----------------------------
-- Sequence structure for protocols_id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."protocols_id_seq";
CREATE SEQUENCE "public"."protocols_id_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1;

-- ----------------------------
-- Table structure for auth_role_claims
-- ----------------------------
DROP TABLE IF EXISTS "public"."auth_role_claims";
CREATE TABLE "public"."auth_role_claims" (
  "id" int4 NOT NULL GENERATED BY DEFAULT AS IDENTITY (
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1
),
  "roleid" text COLLATE "pg_catalog"."default" NOT NULL,
  "claimtype" text COLLATE "pg_catalog"."default",
  "claimvalue" text COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Records of auth_role_claims
-- ----------------------------

-- ----------------------------
-- Table structure for auth_roles
-- ----------------------------
DROP TABLE IF EXISTS "public"."auth_roles";
CREATE TABLE "public"."auth_roles" (
  "id" text COLLATE "pg_catalog"."default" NOT NULL,
  "name" varchar(256) COLLATE "pg_catalog"."default",
  "normalizedname" varchar(256) COLLATE "pg_catalog"."default",
  "concurrencystamp" text COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Records of auth_roles
-- ----------------------------

-- ----------------------------
-- Table structure for auth_user_claims
-- ----------------------------
DROP TABLE IF EXISTS "public"."auth_user_claims";
CREATE TABLE "public"."auth_user_claims" (
  "id" int4 NOT NULL GENERATED BY DEFAULT AS IDENTITY (
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1
),
  "userid" text COLLATE "pg_catalog"."default" NOT NULL,
  "claimtype" text COLLATE "pg_catalog"."default",
  "claimvalue" text COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Records of auth_user_claims
-- ----------------------------

-- ----------------------------
-- Table structure for auth_user_logins
-- ----------------------------
DROP TABLE IF EXISTS "public"."auth_user_logins";
CREATE TABLE "public"."auth_user_logins" (
  "loginprovider" text COLLATE "pg_catalog"."default" NOT NULL,
  "providerkey" text COLLATE "pg_catalog"."default" NOT NULL,
  "providerdisplayname" text COLLATE "pg_catalog"."default",
  "userid" text COLLATE "pg_catalog"."default" NOT NULL
)
;

-- ----------------------------
-- Records of auth_user_logins
-- ----------------------------

-- ----------------------------
-- Table structure for auth_user_roles
-- ----------------------------
DROP TABLE IF EXISTS "public"."auth_user_roles";
CREATE TABLE "public"."auth_user_roles" (
  "userid" text COLLATE "pg_catalog"."default" NOT NULL,
  "roleid" text COLLATE "pg_catalog"."default" NOT NULL
)
;

-- ----------------------------
-- Records of auth_user_roles
-- ----------------------------

-- ----------------------------
-- Table structure for auth_user_tokens
-- ----------------------------
DROP TABLE IF EXISTS "public"."auth_user_tokens";
CREATE TABLE "public"."auth_user_tokens" (
  "userid" text COLLATE "pg_catalog"."default" NOT NULL,
  "loginprovider" text COLLATE "pg_catalog"."default" NOT NULL,
  "name" text COLLATE "pg_catalog"."default" NOT NULL,
  "value" text COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Records of auth_user_tokens
-- ----------------------------

-- ----------------------------
-- Table structure for auth_users
-- ----------------------------
DROP TABLE IF EXISTS "public"."auth_users";
CREATE TABLE "public"."auth_users" (
  "id" text COLLATE "pg_catalog"."default" NOT NULL,
  "username" varchar(256) COLLATE "pg_catalog"."default",
  "normalizedusername" varchar(256) COLLATE "pg_catalog"."default",
  "email" varchar(256) COLLATE "pg_catalog"."default",
  "normalizedemail" varchar(256) COLLATE "pg_catalog"."default",
  "emailconfirmed" bool NOT NULL,
  "passwordhash" text COLLATE "pg_catalog"."default",
  "securitystamp" text COLLATE "pg_catalog"."default",
  "concurrencystamp" text COLLATE "pg_catalog"."default",
  "phonenumber" text COLLATE "pg_catalog"."default",
  "phonenumberconfirmed" bool NOT NULL,
  "twofactorenabled" bool NOT NULL,
  "lockoutend" timestamptz(6),
  "lockoutenabled" bool NOT NULL,
  "accessfailedcount" int4 NOT NULL
)
;

-- ----------------------------
-- Records of auth_users
-- ----------------------------

-- ----------------------------
-- Table structure for logs
-- ----------------------------
DROP TABLE IF EXISTS "public"."logs";
CREATE TABLE "public"."logs" (
  "message" text COLLATE "pg_catalog"."default",
  "message_template" text COLLATE "pg_catalog"."default",
  "level" int4,
  "timestamp" timestamp(6),
  "exception" text COLLATE "pg_catalog"."default",
  "log_event" jsonb
)
;

-- ----------------------------
-- Records of logs
-- ----------------------------
INSERT INTO "public"."logs" VALUES ('Start rabbitmq Protocol - API API versão 1.0', 'Start rabbitmq Protocol - API API versão 1.0', 2, '2024-10-25 18:50:07.271824', NULL, '{"Level": "Information", "Timestamp": "2024-10-25T18:50:07.2718245+00:00", "MessageTemplate": "Start rabbitmq Protocol - API API versão 1.0"}');

-- ----------------------------
-- Table structure for protocols
-- ----------------------------
DROP TABLE IF EXISTS "public"."protocols";
CREATE TABLE "public"."protocols" (
  "id" int4 NOT NULL GENERATED BY DEFAULT AS IDENTITY (
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1
),
  "numprotocol" varchar(50) COLLATE "pg_catalog"."default" NOT NULL,
  "numviadocumento" int4 NOT NULL,
  "cpf" varchar(11) COLLATE "pg_catalog"."default" NOT NULL,
  "rg" varchar(20) COLLATE "pg_catalog"."default" NOT NULL,
  "nome" varchar(200) COLLATE "pg_catalog"."default" NOT NULL,
  "nomemae" varchar(200) COLLATE "pg_catalog"."default" NOT NULL,
  "nomepai" varchar(200) COLLATE "pg_catalog"."default" NOT NULL,
  "foto" bytea
)
;

-- ----------------------------
-- Records of protocols
-- ----------------------------

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
ALTER SEQUENCE "public"."auth_role_claims_id_seq"
OWNED BY "public"."auth_role_claims"."id";
SELECT setval('"public"."auth_role_claims_id_seq"', 1, false);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
ALTER SEQUENCE "public"."auth_user_claims_id_seq"
OWNED BY "public"."auth_user_claims"."id";
SELECT setval('"public"."auth_user_claims_id_seq"', 1, false);

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
ALTER SEQUENCE "public"."protocols_id_seq"
OWNED BY "public"."protocols"."id";
SELECT setval('"public"."protocols_id_seq"', 1, false);

-- ----------------------------
-- Auto increment value for auth_role_claims
-- ----------------------------
SELECT setval('"public"."auth_role_claims_id_seq"', 1, false);

-- ----------------------------
-- Indexes structure for table auth_role_claims
-- ----------------------------
CREATE INDEX "IX_auth_role_claims_roleid" ON "public"."auth_role_claims" USING btree (
  "roleid" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table auth_role_claims
-- ----------------------------
ALTER TABLE "public"."auth_role_claims" ADD CONSTRAINT "PK_auth_role_claims" PRIMARY KEY ("id");

-- ----------------------------
-- Indexes structure for table auth_roles
-- ----------------------------
CREATE UNIQUE INDEX "RoleNameIndex" ON "public"."auth_roles" USING btree (
  "normalizedname" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table auth_roles
-- ----------------------------
ALTER TABLE "public"."auth_roles" ADD CONSTRAINT "PK_auth_roles" PRIMARY KEY ("id");

-- ----------------------------
-- Auto increment value for auth_user_claims
-- ----------------------------
SELECT setval('"public"."auth_user_claims_id_seq"', 1, false);

-- ----------------------------
-- Indexes structure for table auth_user_claims
-- ----------------------------
CREATE INDEX "IX_auth_user_claims_userid" ON "public"."auth_user_claims" USING btree (
  "userid" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table auth_user_claims
-- ----------------------------
ALTER TABLE "public"."auth_user_claims" ADD CONSTRAINT "PK_auth_user_claims" PRIMARY KEY ("id");

-- ----------------------------
-- Indexes structure for table auth_user_logins
-- ----------------------------
CREATE INDEX "IX_auth_user_logins_userid" ON "public"."auth_user_logins" USING btree (
  "userid" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table auth_user_logins
-- ----------------------------
ALTER TABLE "public"."auth_user_logins" ADD CONSTRAINT "PK_auth_user_logins" PRIMARY KEY ("loginprovider", "providerkey");

-- ----------------------------
-- Indexes structure for table auth_user_roles
-- ----------------------------
CREATE INDEX "IX_auth_user_roles_roleid" ON "public"."auth_user_roles" USING btree (
  "roleid" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table auth_user_roles
-- ----------------------------
ALTER TABLE "public"."auth_user_roles" ADD CONSTRAINT "PK_auth_user_roles" PRIMARY KEY ("userid", "roleid");

-- ----------------------------
-- Primary Key structure for table auth_user_tokens
-- ----------------------------
ALTER TABLE "public"."auth_user_tokens" ADD CONSTRAINT "PK_auth_user_tokens" PRIMARY KEY ("userid", "loginprovider", "name");

-- ----------------------------
-- Indexes structure for table auth_users
-- ----------------------------
CREATE INDEX "EmailIndex" ON "public"."auth_users" USING btree (
  "normalizedemail" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE UNIQUE INDEX "UserNameIndex" ON "public"."auth_users" USING btree (
  "normalizedusername" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Primary Key structure for table auth_users
-- ----------------------------
ALTER TABLE "public"."auth_users" ADD CONSTRAINT "PK_auth_users" PRIMARY KEY ("id");

-- ----------------------------
-- Auto increment value for protocols
-- ----------------------------
SELECT setval('"public"."protocols_id_seq"', 1, false);

-- ----------------------------
-- Indexes structure for table protocols
-- ----------------------------
CREATE INDEX "IX_protocols_cpf" ON "public"."protocols" USING btree (
  "cpf" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE INDEX "IX_protocols_numprotocol" ON "public"."protocols" USING btree (
  "numprotocol" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);
CREATE INDEX "IX_protocols_rg" ON "public"."protocols" USING btree (
  "rg" COLLATE "pg_catalog"."default" "pg_catalog"."text_ops" ASC NULLS LAST
);

-- ----------------------------
-- Uniques structure for table protocols
-- ----------------------------
ALTER TABLE "public"."protocols" ADD CONSTRAINT "AK_protocols_numprotocol_cpf" UNIQUE ("numprotocol", "cpf");

-- ----------------------------
-- Checks structure for table protocols
-- ----------------------------
ALTER TABLE "public"."protocols" ADD CONSTRAINT "chk_document_way" CHECK (numviadocumento > 0);
ALTER TABLE "public"."protocols" ADD CONSTRAINT "chk_cpf" CHECK (char_length(cpf::text) = 11);

-- ----------------------------
-- Primary Key structure for table protocols
-- ----------------------------
ALTER TABLE "public"."protocols" ADD CONSTRAINT "PK_protocols" PRIMARY KEY ("id");

-- ----------------------------
-- Foreign Keys structure for table auth_role_claims
-- ----------------------------
ALTER TABLE "public"."auth_role_claims" ADD CONSTRAINT "FK_auth_role_claims_auth_roles_roleid" FOREIGN KEY ("roleid") REFERENCES "public"."auth_roles" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;

-- ----------------------------
-- Foreign Keys structure for table auth_user_claims
-- ----------------------------
ALTER TABLE "public"."auth_user_claims" ADD CONSTRAINT "FK_auth_user_claims_auth_users_userid" FOREIGN KEY ("userid") REFERENCES "public"."auth_users" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;

-- ----------------------------
-- Foreign Keys structure for table auth_user_logins
-- ----------------------------
ALTER TABLE "public"."auth_user_logins" ADD CONSTRAINT "FK_auth_user_logins_auth_users_userid" FOREIGN KEY ("userid") REFERENCES "public"."auth_users" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;

-- ----------------------------
-- Foreign Keys structure for table auth_user_roles
-- ----------------------------
ALTER TABLE "public"."auth_user_roles" ADD CONSTRAINT "FK_auth_user_roles_auth_roles_roleid" FOREIGN KEY ("roleid") REFERENCES "public"."auth_roles" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;
ALTER TABLE "public"."auth_user_roles" ADD CONSTRAINT "FK_auth_user_roles_auth_users_userid" FOREIGN KEY ("userid") REFERENCES "public"."auth_users" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;

-- ----------------------------
-- Foreign Keys structure for table auth_user_tokens
-- ----------------------------
ALTER TABLE "public"."auth_user_tokens" ADD CONSTRAINT "FK_auth_user_tokens_auth_users_userid" FOREIGN KEY ("userid") REFERENCES "public"."auth_users" ("id") ON DELETE CASCADE ON UPDATE NO ACTION;