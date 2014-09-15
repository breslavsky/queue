-- _client_request

ALTER TABLE _client_request DROP CONSTRAINT ClientRequestToClientReference;
ALTER TABLE _client_request ADD CONSTRAINT ClientRequestToClientReference FOREIGN KEY(ClientId)
REFERENCES _client (Id)
ON DELETE CASCADE;

ALTER TABLE _client_request DROP CONSTRAINT ClientRequestToOperatorReference;
ALTER TABLE _client_request ADD CONSTRAINT ClientRequestToOperatorReference FOREIGN KEY(OperatorId)
REFERENCES _user (Id)
ON DELETE CASCADE;

ALTER TABLE _client_request DROP CONSTRAINT ClientRequestToServiceReference;
ALTER TABLE _client_request ADD CONSTRAINT ClientRequestToServiceReference FOREIGN KEY(ServiceId)
REFERENCES _service (Id)
ON DELETE CASCADE;

-- end _client_request

-- _client_request_event

ALTER TABLE _client_request_event DROP CONSTRAINT ClientRequestEventToClientRequestReference;
ALTER TABLE _client_request_event ADD CONSTRAINT ClientRequestEventToClientRequestReference FOREIGN KEY(ClientRequestId)
REFERENCES _client_request (Id)
ON DELETE CASCADE;

ALTER TABLE _client_request_event DROP CONSTRAINT ClientRequestEventToEventReference;
ALTER TABLE _client_request_event ADD CONSTRAINT ClientRequestEventToEventReference FOREIGN KEY(EventId)
REFERENCES _event (Id)
ON DELETE CASCADE;

-- end _client_request_event