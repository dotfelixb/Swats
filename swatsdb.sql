-- Empty Guid
-- 00000000-0000-0000-0000-000000000000

-- anonymous ticket requester

CREATE DATABASE swats;

-- sequences
CREATE SEQUENCE TicketCode INCREMENT 1 START 1;
CREATE SEQUENCE DepartmentCode INCREMENT 1 START 1;

CREATE TABLE authuser 
(
	id BPCHAR(50) PRIMARY KEY
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
	, rowversion BPCHAR(50) NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby BPCHAR(50)
	, updatedat TIMESTAMPTZ DEFAULT(now())
);

CREATE TABLE authuserauditlog
(
    id BPCHAR(50) PRIMARY KEY
	, target BPCHAR(50)
	, actionname VARCHAR(50) NOT NULL
	, description VARCHAR(150) NOT NULL
	, objectname VARCHAR(50) NOT NULL
	, objectdata VARCHAR NOT NULL
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (target) REFERENCES authuser(id) ON DELETE CASCADE
);

CREATE TABLE authrole
(
    id BPCHAR(50) PRIMARY KEY
    , name VARCHAR(150)
	, normalizedname VARCHAR(150)
	, rowversion BPCHAR(50) NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby BPCHAR(50)
	, updatedat TIMESTAMPTZ DEFAULT(now())
);

CREATE TABLE authroleauditlog
(
    id BPCHAR(50) PRIMARY KEY
	, target BPCHAR(50) NOT NULL
	, actionname VARCHAR(50) NOT NULL
	, description VARCHAR(150) NOT NULL
	, objectname VARCHAR(50) NOT NULL
	, objectdata VARCHAR NOT NULL
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (target) REFERENCES authrole(id) ON DELETE CASCADE
);

CREATE TABLE authuserrole
(
    id BPCHAR(50) PRIMARY KEY
    , authuser BPCHAR(50)
	, authrole BPCHAR(50)
	, rowversion BPCHAR(50) NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby BPCHAR(50)
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
	, 'AQAAAAEAACcQAAAAENKZ2rdqa77c2axnGbvBJKuXzMgSi8xDEUH65Rm9S9YitdxK0SlNucoFS3mdi51fGw==' -- root@pAss
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
    id BPCHAR(50) PRIMARY KEY
    , code VARCHAR(20)
    , title VARCHAR(50)
    , requester BPCHAR(50)
    , AssignedTo BPCHAR(50)
    , Source BPCHAR(50)
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby BPCHAR(50)
	, updatedat TIMESTAMPTZ DEFAULT(now())
);

CREATE TABLE Source
(
    id BPCHAR(50) PRIMARY KEY
    , code VARCHAR(10)
    , displayname VARCHAR(50)
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby BPCHAR(50)
	, updatedat TIMESTAMPTZ DEFAULT(now())
);

CREATE TABLE tickettype
(
    id BPCHAR(50) PRIMARY KEY
	, name VARCHAR(50)
	, description VARCHAR
	, color VARCHAR(20)
	, visibility INT
	, rowversion BPCHAR(50) NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby BPCHAR(50)
	, updatedat TIMESTAMPTZ DEFAULT(now())
);

CREATE TABLE tickettypeauditlog
(
    id BPCHAR(50) PRIMARY KEY
	, target BPCHAR(50)
	, actionname VARCHAR(50) 
	, description VARCHAR(150) 
	, objectname VARCHAR(50) 
	, objectdata VARCHAR 
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (target) REFERENCES tickettype(id) ON DELETE CASCADE
);

CREATE TABLE businesshour
(
    id bpchar(50) PRIMARY KEY
	, name VARCHAR(50)
	, description VARCHAR
	, timezone VARCHAR(50)
	, status INT
	, holidays VARCHAR[][]
	, rowversion BPCHAR(50) NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby UUID
	, updatedat TIMESTAMPTZ DEFAULT(now())
);

CREATE TABLE businesshourauditlog
(
    id BPCHAR(50) PRIMARY KEY
	, target bpchar(50) NOT NULL
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
    id bpchar(50) PRIMARY KEY
	, email VARCHAR(50)
	, firstname VARCHAR(50)
	, lastname VARCHAR(50)
	, mobile VARCHAR(50)
	, telephone VARCHAR(50)
	, timezone VARCHAR(50)
	, tickettype BPCHAR(50)
	, mode INT
	, rowversion BPCHAR(50) NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby BPCHAR(50)
	, updatedat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (id) REFERENCES authuser(id) ON DELETE CASCADE
	, FOREIGN KEY (tickettype) REFERENCES tickettype(id) ON DELETE CASCADE
);

CREATE TABLE agentauditlog
(
    id BPCHAR(50) PRIMARY KEY
	, target BPCHAR(50)
	, actionname VARCHAR(50) NOT NULL
	, description VARCHAR(150) NOT NULL
	, objectname VARCHAR(50) NOT NULL
	, objectdata VARCHAR NOT NULL
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (target) REFERENCES department(id) ON DELETE CASCADE
);

CREATE TABLE department
(
    id bpchar(50) PRIMARY KEY
	, code VARCHAR(10)
	, name VARCHAR(50)
	, manager bpchar(50)
	, businesshour bpchar(50)
	, outgoingemail VARCHAR(50)
	, type INT
	, response VARCHAR
	, rowversion BPCHAR(50) NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby BPCHAR(50)
	, updatedat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (businesshour) REFERENCES businesshour(id) ON DELETE CASCADE
	, FOREIGN KEY (manager) REFERENCES agent(id) ON DELETE CASCADE
);

CREATE TABLE departmentauditlog
(
    id BPCHAR(50) PRIMARY KEY
	, target BPCHAR(50) NOT NULL
	, actionname VARCHAR(50) NOT NULL
	, description VARCHAR(150) NOT NULL
	, objectname VARCHAR(50) NOT NULL
	, objectdata VARCHAR NOT NULL
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (target) REFERENCES department(id) ON DELETE CASCADE
);

CREATE TABLE agentdepartment
(
	agent BPCHAR(50)
	, department BPCHAR(50)
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (businesshour) REFERENCES businesshour(id) ON DELETE CASCADE
	, FOREIGN KEY (manager) REFERENCES agent(id) ON DELETE CASCADE
);

CREATE TABLE table
(
    id BPCHAR(50) PRIMARY KEY
	, rowversion BPCHAR(50) NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby BPCHAR(50)
	, updatedat TIMESTAMPTZ DEFAULT(now())
);

CREATE TABLE auditlog
(
    id BPCHAR(50) PRIMARY KEY
	, target BPCHAR(50)
	, actionname VARCHAR(50) NOT NULL
	, description VARCHAR(150) NOT NULL
	, objectname VARCHAR(50) NOT NULL
	, objectdata VARCHAR NOT NULL
    , createdby BPCHAR(50)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (target) REFERENCES (id) ON DELETE CASCADE
);