CREATE TABLE `order` (
  order_id char(36) PRIMARY KEY,
  table_id char(36),
  dish_id char(36),
  customer_name varchar(50),
  customer_phone varchar(20),
  order_time datetime
);