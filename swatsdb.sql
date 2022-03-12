-- Empty Guid
-- 00000000-0000-0000-0000-000000000000

-- anonymous ticket requester

CREATE DATABASE swats;

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
    id uuid PRIMARY KEY
    , code VARCHAR(20)
	, deleted BOOLEAN DEFAULT(FALSE)
    , createdby UUID
	, createdat TIMESTAMPTZ DEFAULT(now())
	, updatedby UUID
	, updatedat TIMESTAMPTZ DEFAULT(now())
);