IF EXISTS (SELECT name FROM sysobjects WHERE name = 'ServiceDelete' AND type = 'TR')
DROP TRIGGER ServiceDelete;
-- SEPARATOR
CREATE TRIGGER ServiceDelete
   ON _service AFTER DELETE
AS 
BEGIN
	DELETE FROM _service_step WHERE ServiceId IN (SELECT Id FROM DELETED)
	DELETE FROM _service_weekday_schedule WHERE ServiceId IN (SELECT Id FROM DELETED)
	DELETE FROM _service_exception_schedule WHERE ServiceId IN (SELECT Id FROM DELETED)
	DELETE FROM _service_parameter WHERE ServiceId IN (SELECT Id FROM DELETED)
END;
-- SEPARATOR
IF EXISTS (SELECT name FROM sysobjects WHERE name = 'ClientRequestDelete' AND type = 'TR')
DROP TRIGGER ClientRequestDelete;
-- SEPARATOR
CREATE TRIGGER ClientRequestDelete
   ON _client_request AFTER DELETE
AS 
BEGIN
	DELETE FROM _client_request_event WHERE ClientRequestId IN (SELECT Id FROM DELETED)
	DELETE FROM _client_request_parameter WHERE ClientRequestId IN (SELECT Id FROM DELETED)
END;
-- SEPARATOR
IF EXISTS (SELECT name FROM sysobjects WHERE name = 'ServiceGroupDelete' AND type = 'TR')
DROP TRIGGER ServiceGroupDelete;
-- SEPARATOR
CREATE TRIGGER ServiceGroupDelete
   ON _service_group AFTER DELETE
AS 
BEGIN
	DELETE FROM _service_group WHERE ParentGroupId IN (SELECT Id FROM DELETED)
	DELETE FROM _service WHERE ServiceGroupId IN (SELECT Id FROM DELETED)
END;