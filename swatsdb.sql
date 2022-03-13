-- Empty Guid
-- 00000000-0000-0000-0000-000000000000

-- anonymous ticket requester

CREATE DATABASE swats;

CREATE TABLE authuser 
(
	id UUID PRIMARY KEY
    , username VARCHAR(150)
	, normalizedusername VARCHAR(150)
	, email VARCHAR(50)
	, emailconfirmed BOOLEAN
	, passwordhash VARCHAR
	, securitystamp VARCHAR
	, phone VARCHAR(20)
	, phoneconfirmed BOOLEAN
	, twofactorenabled BOOLEAN
	, lockout BOOLEAN
	, failedcount int
	, rowversion UUID NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby UUID
	, updatedat TIMESTAMPTZ DEFAULT(now())
);

CREATE TABLE authuserauditlog
(
    id UUID PRIMARY KEY
	, target UUID
	, actionname VARCHAR(50) NOT NULL
	, description VARCHAR(150) NOT NULL
	, objectname VARCHAR(50) NOT NULL
	, objectdata VARCHAR NOT NULL
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (target) REFERENCES authuser(id) ON DELETE CASCADE
);

CREATE TABLE authrole
(
    id UUID PRIMARY KEY
    , name VARCHAR(150)
	, normalizedname VARCHAR(150)
	, rowversion UUID NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby UUID
	, updatedat TIMESTAMPTZ DEFAULT(now())
);

CREATE TABLE authroleauditlog
(
    id UUID PRIMARY KEY
	, target UUID NOT NULL
	, actionname VARCHAR(50) NOT NULL
	, description VARCHAR(150) NOT NULL
	, objectname VARCHAR(50) NOT NULL
	, objectdata VARCHAR NOT NULL
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (target) REFERENCES authrole(id) ON DELETE CASCADE
);

CREATE TABLE authuserrole
(
    id UUID PRIMARY KEY
    , authuser UUID
	, authrole UUID
	, rowversion UUID NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby UUID
	, updatedat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (authuser) REFERENCES authuser(id) ON DELETE CASCADE
	, FOREIGN KEY (authrole) REFERENCES authrole(id) ON DELETE CASCADE
);

-- create system users



CREATE TABLE ticket
(
    id uuid PRIMARY KEY
    , code VARCHAR(20)
    , title VARCHAR(50)
    , requester UUID
    , AssignedTo UUID
    , Source UUID
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby UUID
	, updatedat TIMESTAMPTZ DEFAULT(now())
);

CREATE TABLE Source
(
    id uuid PRIMARY KEY
    , code VARCHAR(20)
    , displayname VARCHAR(50)
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby UUID
	, updatedat TIMESTAMPTZ DEFAULT(now())
);

CREATE TABLE table
(
    id UUID PRIMARY KEY
	, rowversion UUID NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby UUID
	, updatedat TIMESTAMPTZ DEFAULT(now())
);

CREATE TABLE auditlog
(
    id UUID PRIMARY KEY
	, target UUID NOT NULL
	, actionname VARCHAR(50) NOT NULL
	, description VARCHAR(150) NOT NULL
	, objectname VARCHAR(50) NOT NULL
	, objectdata VARCHAR NOT NULL
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
);