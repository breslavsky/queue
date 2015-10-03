ALTER TABLE _client_request DROP CONSTRAINT ClientRequestToClientReference;
-- SEPARATOR
ALTER TABLE _client_request ADD CONSTRAINT ClientRequestToClientReference FOREIGN KEY(ClientId)
REFERENCES _client (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _client_request DROP CONSTRAINT ClientRequestToOperatorReference;
-- SEPARATOR
ALTER TABLE _client_request ADD CONSTRAINT ClientRequestToOperatorReference FOREIGN KEY(OperatorId)
REFERENCES _user (Id)
ON DELETE SET NULL;
-- SEPARATOR
ALTER TABLE _client_request DROP CONSTRAINT ClientRequestToServiceReference;
-- SEPARATOR
ALTER TABLE _client_request ADD CONSTRAINT ClientRequestToServiceReference FOREIGN KEY(ServiceId)
REFERENCES _service (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _client_request DROP CONSTRAINT ClientRequestToServiceStepReference;
-- SEPARATOR
ALTER TABLE _client_request ADD CONSTRAINT ClientRequestToServiceStepReference FOREIGN KEY(StepId)
REFERENCES _service_step (Id)
ON DELETE NO ACTION;
-- SEPARATOR
ALTER TABLE _client_request_event DROP CONSTRAINT ClientRequestEventToClientRequestReference;
-- SEPARATOR
ALTER TABLE _client_request_event ADD CONSTRAINT ClientRequestEventToClientRequestReference FOREIGN KEY(ClientRequestId)
REFERENCES _client_request (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _client_request_event DROP CONSTRAINT ClientRequestEventToEventReference;
-- SEPARATOR
ALTER TABLE _client_request_event ADD CONSTRAINT ClientRequestEventToEventReference FOREIGN KEY(EventId)
REFERENCES _event (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _client_request_parameter DROP CONSTRAINT ClientRequestParameterToClientRequestReference;
-- SEPARATOR
ALTER TABLE _client_request_parameter ADD CONSTRAINT ClientRequestParameterToClientRequestReference FOREIGN KEY(ClientRequestId)
REFERENCES _client_request (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _service DROP CONSTRAINT ServiceToServiceGroupReference;
-- SEPARATOR
ALTER TABLE _service ADD CONSTRAINT ServiceToServiceGroupReference FOREIGN KEY(ServiceGroupId)
REFERENCES _service_group (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _service_parameter DROP CONSTRAINT ServiceParameterToServiceReference;
-- SEPARATOR
ALTER TABLE _service_parameter ADD CONSTRAINT ServiceParameterToServiceReference FOREIGN KEY(ServiceId)
REFERENCES _service (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _service_step DROP CONSTRAINT ServiceStepToServiceReference;
-- SEPARATOR
ALTER TABLE _service_step ADD CONSTRAINT ServiceStepToServiceReference FOREIGN KEY(ServiceId)
REFERENCES _service (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _service_rendering DROP CONSTRAINT ServiceRenderingToOperatorReference;
-- SEPARATOR
ALTER TABLE _service_rendering ADD CONSTRAINT ServiceRenderingToOperatorReference FOREIGN KEY(OperatorId)
REFERENCES _user (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _service_rendering DROP CONSTRAINT ServiceRenderingToScheduleReference;
-- SEPARATOR
ALTER TABLE _service_rendering ADD CONSTRAINT ServiceRenderingToScheduleReference FOREIGN KEY(ScheduleId)
REFERENCES _schedule (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _service_rendering DROP CONSTRAINT ServiceRenderingToServiceStepReference;
-- SEPARATOR
ALTER TABLE _service_rendering ADD CONSTRAINT ServiceRenderingToServiceStepReference FOREIGN KEY(ServiceStepId)
REFERENCES _service_step (Id)
ON DELETE SET NULL;
-- SEPARATOR
ALTER TABLE _service_weekday_schedule DROP CONSTRAINT ServiceWeekdayScheduleToScheduleReference;
-- SEPARATOR
ALTER TABLE _service_weekday_schedule ADD CONSTRAINT ServiceWeekdayScheduleToScheduleReference FOREIGN KEY(ScheduleId)
REFERENCES _schedule (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _service_weekday_schedule DROP CONSTRAINT ServiceWeekdayScheduleToServiceReference;
-- SEPARATOR
ALTER TABLE _service_weekday_schedule ADD CONSTRAINT ServiceWeekdayScheduleToServiceReference FOREIGN KEY(ServiceId)
REFERENCES _service (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _service_exception_schedule DROP CONSTRAINT ServiceExceptionScheduleToScheduleReference;
-- SEPARATOR
ALTER TABLE _service_exception_schedule ADD CONSTRAINT ServiceExceptionScheduleToScheduleReference FOREIGN KEY(ScheduleId)
REFERENCES _schedule (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _service_exception_schedule DROP CONSTRAINT ServiceExceptionScheduleToServiceReference;
-- SEPARATOR
ALTER TABLE _service_exception_schedule ADD CONSTRAINT ServiceExceptionScheduleToServiceReference FOREIGN KEY(ServiceId)
REFERENCES _service (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _user DROP CONSTRAINT OperatorToWorkplaceReference;
-- SEPARATOR
ALTER TABLE _user ADD CONSTRAINT OperatorToWorkplaceReference FOREIGN KEY(WorkplaceId)
REFERENCES _workplace (Id)
ON DELETE SET NULL;
-- SEPARATOR
ALTER TABLE _user_event DROP CONSTRAINT UserEventToEventReference;
-- SEPARATOR
ALTER TABLE _user_event ADD CONSTRAINT UserEventToEventReference FOREIGN KEY(EventId)
REFERENCES _event (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _user_event DROP CONSTRAINT UserEventToUserReference;
-- SEPARATOR
ALTER TABLE _user_event ADD CONSTRAINT UserEventToUserReference FOREIGN KEY(UserId)
REFERENCES _user (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _user_event DROP CONSTRAINT UserEventToEventReference;
-- SEPARATOR
ALTER TABLE _user_event ADD CONSTRAINT UserEventToEventReference FOREIGN KEY(EventId)
REFERENCES _event (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _user_event DROP CONSTRAINT UserEventToUserReference;
-- SEPARATOR
ALTER TABLE _user_event ADD CONSTRAINT UserEventToUserReference FOREIGN KEY(UserId)
REFERENCES _user (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _operator_interruption DROP CONSTRAINT OperatorInterruptionToOperatorReference;
-- SEPARATOR
ALTER TABLE _operator_interruption ADD CONSTRAINT OperatorInterruptionToOperatorReference FOREIGN KEY(OperatorId)
REFERENCES _user (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _client_request_additional_service DROP CONSTRAINT ClientRequestAdditionalServiceToAdditionalServiceReference;
-- SEPARATOR
ALTER TABLE _client_request_additional_service ADD CONSTRAINT ClientRequestAdditionalServiceToAdditionalServiceReference FOREIGN KEY(AdditionalServiceId)
REFERENCES _additional_service (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _client_request_additional_service DROP CONSTRAINT ClientRequestAdditionalServiceToOperatorReference;
-- SEPARATOR
ALTER TABLE _client_request_additional_service ADD CONSTRAINT ClientRequestAdditionalServiceToOperatorReference FOREIGN KEY(OperatorId)
REFERENCES _user (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _client_request_additional_service DROP CONSTRAINT ClientRequestAdditionalServiceToClientRequestReference;
-- SEPARATOR
ALTER TABLE _client_request_additional_service ADD CONSTRAINT ClientRequestAdditionalServiceToClientRequestReference FOREIGN KEY(ClientRequestId)
REFERENCES _client_request (Id)
ON DELETE CASCADE;
-- SEPARATOR
ALTER TABLE _queue_plan_report DROP CONSTRAINT QueuePlanReportToClientRequestReference;
-- SEPARATOR
ALTER TABLE _queue_plan_report ADD  CONSTRAINT QueuePlanReportToClientRequestReference FOREIGN KEY(ClientRequestId)
REFERENCES _client_request (Id)
ON DELETE CASCADE;
