-- Empty Guid
-- 00000000-0000-0000-0000-000000000000

-- anonymous ticket requester

CREATE DATABASE swats;

-- sequences
CREATE SEQUENCE TicketCode INCREMENT 1 START 1;

CREATE TABLE authuser 
(
	id UUID PRIMARY KEY
    , username VARCHAR(150)
	, normalizedusername VARCHAR(150)
	, email VARCHAR(150)
	, normalizedemail VARCHAR(150)
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
INSERT INTO public.authuser 
	(id
	, username
	, normalizedusername
	, email
	, normalizedemail
	, emailconfirmed
	, passwordhash
	, securitystamp
	, phone
	, phoneconfirmed
	, twofactorenabled
	, lockout
	, failedcount
	, rowversion
	, createdby
	, updatedby)
VALUES
	('00000000-0000-0000-0000-000000000001'
	, 'system'
	, 'SYSTEM'
	, 'system@swats.email'
	, 'SYSTEM@SWATS.APP'
	, FALSE
	, 'AQAAAAEAACcQAAAAENKZ2rdqa77c2axnGbvBJKuXzMgSi8xDEUH65Rm9S9YitdxK0SlNucoFS3mdi51fGw=='
	, '00000000000000000000000000000001'
	, '0000000000'
	, FALSE
	, FALSE
	, FALSE
	, 0
	, '00000000-0000-0000-0000-000000000001'
	, '00000000-0000-0000-0000-000000000001'
	, '00000000-0000-0000-0000-000000000001');



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

CREATE TABLE tickettype
(
    id UUID PRIMARY KEY
	, name VARCHAR(50)
	, description VARCHAR
	, color VARCHAR(20)
	, visibility INT
	, rowversion UUID NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby UUID
	, updatedat TIMESTAMPTZ DEFAULT(now())
);

CREATE TABLE tickettypeauditlog
(
    id UUID PRIMARY KEY
	, target UUID
	, actionname VARCHAR(50) 
	, description VARCHAR(150) 
	, objectname VARCHAR(50) 
	, objectdata VARCHAR 
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (target) REFERENCES tickettype(id) ON DELETE CASCADE
);

CREATE TABLE businesshour
(
    id UUID PRIMARY KEY
	, name VARCHAR(50)
	, description VARCHAR
	, timezone VARCHAR(50)
	, status INT
	, holidays VARCHAR[][]
	, rowversion UUID NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby UUID
	, updatedat TIMESTAMPTZ DEFAULT(now())
);


CREATE TABLE businesshourauditlog
(
    id UUID PRIMARY KEY
	, target UUID NOT NULL
	, actionname VARCHAR(50) NOT NULL
	, description VARCHAR(150) NOT NULL
	, objectname VARCHAR(50) NOT NULL
	, objectdata VARCHAR NOT NULL
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (target) REFERENCES businesshour(id) ON DELETE CASCADE
);

CREATE TABLE agent
(
    id UUID PRIMARY KEY
	, email VARCHAR(50)
	, firstname VARCHAR(50)
	, lastname VARCHAR(50)
	, mobile VARCHAR(50)
	, telephone VARCHAR(50)
	, timezone VARCHAR(50)
	, department UUID
	, team UUID
	, tickettype UUID
	, mode INT
	, rowversion UUID NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby UUID
	, updatedat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (id) REFERENCES authuser(id) ON DELETE CASCADE
	, FOREIGN KEY (department) REFERENCES department(id) ON DELETE CASCADE
	, FOREIGN KEY (team) REFERENCES team(id) ON DELETE CASCADE
	, FOREIGN KEY (tickettype) REFERENCES tickettype(id) ON DELETE CASCADE
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
	, FOREIGN KEY (target) REFERENCES (id) ON DELETE CASCADE
);