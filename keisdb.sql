-- Empty Guid
-- 00000000-0000-0000-0000-000000000000

-- anonymous ticket requester

-- CREATE DATABASE keisdesk;


-- sequences
CREATE SEQUENCE TicketCode INCREMENT 1 START 1;
CREATE SEQUENCE DepartmentCode INCREMENT 1 START 1;

CREATE TABLE authuser
(
    id                 BPCHAR(36) PRIMARY KEY,
    username           VARCHAR(150),
    normalizedusername VARCHAR(150),
    email              VARCHAR(150),
    normalizedemail    VARCHAR(150),
    emailconfirmed     BOOLEAN,
    passwordhash       VARCHAR,
    securitystamp      VARCHAR,
    phone              VARCHAR(20),
    phoneconfirmed     BOOLEAN,
    twofactorenabled   BOOLEAN,
    lockout            BOOLEAN,
    failedcount        int,
    rowversion         BPCHAR(36) NOT NULL,
    deleted            BOOLEAN     DEFAULT (FALSE),
    createdby          BPCHAR(36),
    createdat          TIMESTAMPTZ DEFAULT (now()),
    updatedby          BPCHAR(36),
    updatedat          TIMESTAMPTZ DEFAULT (now())
);

CREATE TABLE authuserauditlog
(
    id          BPCHAR(36) PRIMARY KEY,
    target      BPCHAR(36),
    actionname  VARCHAR(50)  NOT NULL,
    description VARCHAR(150) NOT NULL,
    objectname  VARCHAR(50)  NOT NULL,
    objectdata  VARCHAR      NOT NULL,
    createdby   BPCHAR(36),
    createdat   TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (target) REFERENCES authuser (id) ON DELETE CASCADE
);

CREATE TABLE authrole
(
    id             BPCHAR(36) PRIMARY KEY,
    name           VARCHAR(150),
    normalizedname VARCHAR(150),
    status         INT,
    rowversion     BPCHAR(36) NOT NULL,
    deleted        BOOLEAN     DEFAULT (FALSE),
    createdby      BPCHAR(36),
    createdat      TIMESTAMPTZ DEFAULT (now()),
    updatedby      BPCHAR(36),
    updatedat      TIMESTAMPTZ DEFAULT (now())
);

CREATE TABLE authroleauditlog
(
    id          BPCHAR(36) PRIMARY KEY,
    target      BPCHAR(36)   NOT NULL,
    actionname  VARCHAR(50)  NOT NULL,
    description VARCHAR(150) NOT NULL,
    objectname  VARCHAR(50)  NOT NULL,
    objectdata  VARCHAR      NOT NULL,
    createdby   BPCHAR(36),
    createdat   TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (target) REFERENCES authrole (id) ON DELETE CASCADE
);

CREATE TABLE authuserrole
(
    id         BPCHAR(36) PRIMARY KEY,
    authuser   BPCHAR(36),
    authrole   BPCHAR(36),
    rowversion BPCHAR(36) NOT NULL,
    deleted    BOOLEAN     DEFAULT (FALSE),
    createdby  BPCHAR(36),
    createdat  TIMESTAMPTZ DEFAULT (now()),
    updatedby  BPCHAR(36),
    updatedat  TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (authuser) REFERENCES authuser (id) ON DELETE CASCADE,
    FOREIGN KEY (authrole) REFERENCES authrole (id) ON DELETE CASCADE
);

CREATE TABLE authloginauditlog
(
    id         BPCHAR(36) PRIMARY KEY,
    authuser   BPCHAR(36),
    device     VARCHAR (50),
    platform   VARCHAR (50),
    browser    VARCHAR (50),
    address    VARCHAR (50),
    loginat    TIMESTAMPTZ DEFAULT (now()),
    createdby  BPCHAR(36) DEFAULT ('00000000-0000-0000-0000-000000000001'),
    createdat  TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (authuser) REFERENCES authuser (id) ON DELETE CASCADE
);


-- create system users
INSERT INTO public.authuser
( id
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
VALUES ( '00000000-0000-0000-0000-000000000001'
       , 'system'
       , 'SYSTEM'
       , 'system@Keis.email'
       , 'SYSTEM@KEIS.APP'
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


CREATE TABLE tickettype
(
    id          BPCHAR(36) PRIMARY KEY,
    name        VARCHAR(50),
    description VARCHAR,
    color       VARCHAR(20),
    visibility  INT,
    status      INT,
    rowversion  BPCHAR(36) NOT NULL,
    deleted     BOOLEAN     DEFAULT (FALSE),
    createdby   BPCHAR(36),
    createdat   TIMESTAMPTZ DEFAULT (now()),
    updatedby   BPCHAR(36),
    updatedat   TIMESTAMPTZ DEFAULT (now())
);

CREATE TABLE tickettypeauditlog
(
    id          BPCHAR(36) PRIMARY KEY,
    target      BPCHAR(36),
    actionname  VARCHAR(50),
    description VARCHAR(150),
    objectname  VARCHAR(50),
    objectdata  VARCHAR,
    createdby   BPCHAR(36),
    createdat   TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (target) REFERENCES tickettype (id) ON DELETE CASCADE
);

CREATE TABLE businesshour
(
    id          BPCHAR(36) PRIMARY KEY,
    name        VARCHAR(50),
    description VARCHAR,
    timezone    VARCHAR(50),
    holidays    VARCHAR[][],
    status      INT,
    rowversion  BPCHAR(36) NOT NULL,
    deleted     BOOLEAN     DEFAULT (FALSE),
    createdby   BPCHAR(36),
    createdat   TIMESTAMPTZ DEFAULT (now()),
    updatedby   BPCHAR(36),
    updatedat   TIMESTAMPTZ DEFAULT (now())
);

CREATE TABLE businessopenhour
(
    id          		BPCHAR(36) PRIMARY KEY,
    businesshour  	BPCHAR(36),
    name        		VARCHAR(50),
    enabled     		BOOLEAN     DEFAULT (FALSE),
    fullday			BOOLEAN  DEFAULT (FALSE),
    fromtime    		TIMESTAMP,
    totime      		TIMESTAMP,
    FOREIGN KEY (businesshour) REFERENCES businesshour (id) ON DELETE CASCADE
);

CREATE TABLE businesshourauditlog
(
    id          BPCHAR(36) PRIMARY KEY,
    target      BPCHAR(36)   NOT NULL,
    actionname  VARCHAR(50)  NOT NULL,
    description VARCHAR(150) NOT NULL,
    objectname  VARCHAR(50)  NOT NULL,
    objectdata  VARCHAR      NOT NULL,
    createdby   BPCHAR(36),
    createdat   TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (target) REFERENCES businesshour (id) ON DELETE CASCADE
);

CREATE TABLE department
(
    id            BPCHAR(36) PRIMARY KEY,
    code          VARCHAR(10),
    name          VARCHAR(50),
    manager       BPCHAR(36),
    businesshour  BPCHAR(36),
    outgoingemail VARCHAR(50),
    type          INT,
    status        INT,
    response      VARCHAR,
    rowversion    BPCHAR(36) NOT NULL,
    deleted       BOOLEAN     DEFAULT (FALSE),
    createdby     BPCHAR(36),
    createdat     TIMESTAMPTZ DEFAULT (now()),
    updatedby     BPCHAR(36),
    updatedat     TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (businesshour) REFERENCES businesshour (id) ON DELETE CASCADE
);

CREATE TABLE departmentauditlog
(
    id          BPCHAR(36) PRIMARY KEY,
    target      BPCHAR(36)   NOT NULL,
    actionname  VARCHAR(50)  NOT NULL,
    description VARCHAR(150) NOT NULL,
    objectname  VARCHAR(50)  NOT NULL,
    objectdata  VARCHAR      NOT NULL,
    createdby   BPCHAR(36),
    createdat   TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (target) REFERENCES department (id) ON DELETE CASCADE
);

CREATE TABLE team
(
    id         BPCHAR(36) PRIMARY KEY,
    name       VARCHAR(50),
    department BPCHAR(36),
    lead       BPCHAR(36),
    status     INT,
    rowversion BPCHAR(36) NOT NULL,
    deleted    BOOLEAN     DEFAULT (FALSE),
    createdby  BPCHAR(36),
    createdat  TIMESTAMPTZ DEFAULT (now()),
    updatedby  BPCHAR(36),
    updatedat  TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (department) REFERENCES department (id) ON DELETE CASCADE
);

CREATE TABLE teamauditlog
(
    id          BPCHAR(36) PRIMARY KEY,
    target      BPCHAR(36),
    actionname  VARCHAR(50)  NOT NULL,
    description VARCHAR(150) NOT NULL,
    objectname  VARCHAR(50)  NOT NULL,
    objectdata  VARCHAR      NOT NULL,
    createdby   BPCHAR(36),
    createdat   TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (target) REFERENCES team (id) ON DELETE CASCADE
);

CREATE TABLE agent
(
    id         BPCHAR(36) PRIMARY KEY,
    email      VARCHAR(50),
    firstname  VARCHAR(50),
    lastname   VARCHAR(50),
    mobile     VARCHAR(50),
    telephone  VARCHAR(50),
    timezone   VARCHAR(50),
    department BPCHAR(36),
    team       BPCHAR(36),
    tickettype BPCHAR(36),
    status     INT,
    mode       INT,
    note       TEXT,
    rowversion BPCHAR(36) NOT NULL,
    deleted    BOOLEAN     DEFAULT (FALSE),
    createdby  BPCHAR(36),
    createdat  TIMESTAMPTZ DEFAULT (now()),
    updatedby  BPCHAR(36),
    updatedat  TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (id) REFERENCES authuser (id) ON DELETE CASCADE,
    FOREIGN KEY (department) REFERENCES department (id) ON DELETE CASCADE,
    FOREIGN KEY (tickettype) REFERENCES tickettype (id) ON DELETE CASCADE
);

ALTER TABLE public.agent
    ADD CONSTRAINT agent_team_fkey FOREIGN KEY (team) REFERENCES team (id) ON DELETE CASCADE;

CREATE TABLE agentauditlog
(
    id          BPCHAR(36) PRIMARY KEY,
    target      BPCHAR(36),
    actionname  VARCHAR(50)  NOT NULL,
    description VARCHAR(150) NOT NULL,
    objectname  VARCHAR(50)  NOT NULL,
    objectdata  VARCHAR      NOT NULL,
    createdby   BPCHAR(36),
    createdat   TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (target) REFERENCES agent (id) ON DELETE CASCADE
);

CREATE TABLE helptopic
(
    id                 BPCHAR(36) PRIMARY KEY,
    topic              VARCHAR(50),
    type               INT,
    department         BPCHAR(36),
    defeaultdepartment BPCHAR(36),
    note               TEXT,
    status             INT,
    rowversion         BPCHAR(36) NOT NULL,
    deleted            BOOLEAN     DEFAULT (FALSE),
    createdby          BPCHAR(36),
    createdat          TIMESTAMPTZ DEFAULT (now()),
    updatedby          BPCHAR(36),
    updatedat          TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (department) REFERENCES department (id) ON DELETE CASCADE,
    FOREIGN KEY (defeaultdepartment) REFERENCES department (id) ON DELETE CASCADE
);

CREATE TABLE helptopicauditlog
(
    id          BPCHAR(36) PRIMARY KEY,
    target      BPCHAR(36),
    actionname  VARCHAR(50)  NOT NULL,
    description VARCHAR(150) NOT NULL,
    objectname  VARCHAR(50)  NOT NULL,
    objectdata  VARCHAR      NOT NULL,
    createdby   BPCHAR(36),
    createdat   TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (target) REFERENCES helptopic (id) ON DELETE CASCADE
);


CREATE TABLE ticket
(
    id            BPCHAR(36) PRIMARY KEY,
    code          VARCHAR(20),
    subject       VARCHAR(50),
    requester     VARCHAR(50),
    externalagent VARCHAR(50),
    assignedto    BPCHAR(36),
    source        INT,
    tickettype    BPCHAR(36),
    department    BPCHAR(36),
    team          BPCHAR(36),
    helptopic     BPCHAR(36),
    priority      INT,
    status        INT,
    dueat		  TIMESTAMPTZ,
    rowversion    BPCHAR(36) NOT NULL,
    deleted       BOOLEAN     DEFAULT (FALSE),
    createdby     BPCHAR(36),
    createdat     TIMESTAMPTZ DEFAULT (now()),
    updatedby     BPCHAR(36),
    updatedat     TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (tickettype) REFERENCES tickettype (id) ON DELETE CASCADE,
    FOREIGN KEY (department) REFERENCES department (id) ON DELETE CASCADE,
    FOREIGN KEY (team) REFERENCES team (id) ON DELETE CASCADE,
    FOREIGN KEY (helptopic) REFERENCES helptopic (id) ON DELETE CASCADE
);


CREATE TABLE ticketauditlog
(
    id          BPCHAR(36) PRIMARY KEY,
    target      BPCHAR(36),
    actionname  VARCHAR(50)  NOT NULL,
    description VARCHAR(150) NOT NULL,
    objectname  VARCHAR(50)  NOT NULL,
    objectdata  VARCHAR      NOT NULL,
    createdby   BPCHAR(36),
    createdat   TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (target) REFERENCES ticket (id) ON DELETE CASCADE
);

CREATE TABLE tags
(
    id         BPCHAR(36) PRIMARY KEY,
    name       VARCHAR(50),
    note       TEXT,
    color      VARCHAR(20),
    visibility INT,
    status     INT,
    rowversion BPCHAR(36) NOT NULL,
    deleted    BOOLEAN     DEFAULT (FALSE),
    createdby  BPCHAR(36),
    createdat  TIMESTAMPTZ DEFAULT (now()),
    updatedby  BPCHAR(36),
    updatedat  TIMESTAMPTZ DEFAULT (now())
);


CREATE TABLE tagsauditlog
(
    id          BPCHAR(36) PRIMARY KEY,
    target      BPCHAR(36),
    actionname  VARCHAR(50)  NOT NULL,
    description VARCHAR(150) NOT NULL,
    objectname  VARCHAR(50)  NOT NULL,
    objectdata  VARCHAR      NOT NULL,
    createdby   BPCHAR(36),
    createdat   TIMESTAMPTZ DEFAULT (now()),
    FOREIGN KEY (target) REFERENCES tags (id) ON DELETE CASCADE
);

CREATE TABLE ticketcomment
(
    id BPCHAR(36) PRIMARY KEY,
    ticket 		BPCHAR(36),
    fromemail  VARCHAR(50), -- agent 
    fromname  VARCHAR(50), -- agent 
    toemail	VARCHAR(50), -- requester
    toname	VARCHAR(50), -- requester
    recipients TEXT[][], -- {{'name':'email}, {'name':'email'}}
    body          TEXT,
    commenttype INT DEFAULT(1),
    status     INT DEFAULT(1),
    source     INT DEFAULT(1),
    target  BPCHAR(36) ,
	rowversion BPCHAR(36) NOT NULL,
	deleted BOOLEAN DEFAULT(FALSE),
  	createdby BPCHAR(36),
	createdat TIMESTAMPTZ DEFAULT(now()),
	FOREIGN KEY (ticket) REFERENCES ticket(id) ON DELETE CASCADE
);


CREATE TABLE sla
(
    id BPCHAR(36) PRIMARY KEY
    , name VARCHAR(50) NOT NULL
    , businesshour BPCHAR(36)
    , responseperiod INT
    , responseformat INT
    , responseNotify BOOLEAN
    , responseemail BOOLEAN
    , resolveperiod INT  
    , resolveformat INT
    , resolvenotify BOOLEAN
    , resolveemail BOOLEAN
    , note  TEXT
    , status INT
	, rowversion BPCHAR(36) NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
  	, createdby BPCHAR(36)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby BPCHAR(36)
	, updatedat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (businesshour) REFERENCES businesshour (id) ON DELETE CASCADE
);


CREATE TABLE slaauditlog
(
    id BPCHAR(36) PRIMARY KEY
	, target BPCHAR(36)
	, actionname VARCHAR(50) NOT NULL
	, description VARCHAR(150) NOT NULL
	, objectname VARCHAR(50) NOT NULL
	, objectdata VARCHAR NOT NULL
    , createdby BPCHAR(36)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (target) REFERENCES sla(id) ON DELETE CASCADE
);

CREATE TABLE workflow
(
    id BPCHAR(36) PRIMARY KEY
    , name VARCHAR(50) NOT NULL
    , events INT[]
    , note TEXT
    , priority INT
    , status INT
	, rowversion BPCHAR(36) NOT NULL
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby BPCHAR(36)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby BPCHAR(36)
	, updatedat TIMESTAMPTZ DEFAULT(now())
);

CREATE TABLE workflowaction
(
    id BPCHAR(36) PRIMARY KEY
    , workflow BPCHAR(36)
    , name VARCHAR(50) 
    , actionfrom INT
    , actionto VARCHAR(50)
    , FOREIGN KEY (workflow) REFERENCES workflow(id) ON DELETE CASCADE
);

CREATE TABLE workflowcriteria 
(
    id BPCHAR(36) PRIMARY KEY
    , workflow BPCHAR(36)
    , name VARCHAR(50) 
    , criteria INT
    , condition INT
    , match VARCHAR(50)
    , FOREIGN KEY (workflow) REFERENCES workflow(id) ON DELETE CASCADE
);


CREATE TABLE workflowauditlog
(
    id BPCHAR(36) PRIMARY KEY
	, target BPCHAR(36)
	, actionname VARCHAR(50) NOT NULL
	, description VARCHAR(150) NOT NULL
	, objectname VARCHAR(50) NOT NULL
	, objectdata VARCHAR NOT NULL
    , createdby BPCHAR(36)
	, createdat TIMESTAMPTZ DEFAULT(now())
	, FOREIGN KEY (target) REFERENCES workflow(id) ON DELETE CASCADE
);


--CREATE TABLE table
--(
--    id BPCHAR(36) PRIMARY KEY
--    , status INT
--	, rowversion BPCHAR(36) NOT NULL
--	, deleted BOOLEAN DEFAULT(FALSE)
--  , createdby BPCHAR(36)
--	, createdat TIMESTAMPTZ DEFAULT(now())
--	, updatedby BPCHAR(36)
--	, updatedat TIMESTAMPTZ DEFAULT(now())
--);

--
--CREATE TABLE auditlog
--(
--    id BPCHAR(36) PRIMARY KEY
--	, target BPCHAR(36)
--	, actionname VARCHAR(50) NOT NULL
--	, description VARCHAR(150) NOT NULL
--	, objectname VARCHAR(50) NOT NULL
--	, objectdata VARCHAR NOT NULL
--  , createdby BPCHAR(36)
--	, createdat TIMESTAMPTZ DEFAULT(now())
--	, FOREIGN KEY (target) REFERENCES (id) ON DELETE CASCADE
--);