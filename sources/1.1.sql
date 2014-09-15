truncate table _metric_queue_plan_service;
truncate table _metric_queue_plan_operator;
truncate table _metric_queue_plan;
truncate table _metric;

update _user set [Role] = 'Administrator' where [Role] = 'ADMINISTRATOR';
update _user set [Role] = 'Manager' where [Role] = 'MANAGER';
update _user set [Role] = 'Operator' where [Role] = 'OPERATOR';
alter table _user drop column [Playing];

alter table _client_request drop column [Speed];


update _service set [ClientRequire] = 1;

update _config_terminal set [Columns] = 2, [Rows] = 5;

alter table _office drop column [UserId];
alter table _office drop column [Password];

update _service_group set [Columns] = 2, [Rows] = 5;

update _metric_queue_plan set Productivity = 100;