/*
Navicat MySQL Data Transfer

Source Server         : john
Source Server Version : 50560
Source Host           : localhost:3306
Source Database       : test

Target Server Type    : MYSQL
Target Server Version : 50560
File Encoding         : 65001

Date: 2018-10-29 18:30:34
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for pb_base_type
-- ----------------------------
DROP TABLE IF EXISTS `pb_base_type`;
CREATE TABLE `pb_base_type` (
  `baseTypeId` int(11) NOT NULL AUTO_INCREMENT COMMENT '基础类型id',
  `superBaseTypeId` int(11) DEFAULT '0' COMMENT '上级基础类型id',
  `baseTypeName` varchar(30) NOT NULL COMMENT '基础类型名称',
  `type` int(2) DEFAULT NULL COMMENT '类型，1区域，2设备',
  `description` varchar(100) DEFAULT NULL COMMENT '基础类型描述',
  PRIMARY KEY (`baseTypeId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='基础类型';

-- ----------------------------
-- Table structure for pb_conference
-- ----------------------------
DROP TABLE IF EXISTS `pb_conference`;
CREATE TABLE `pb_conference` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '党会议id',
  `hall_id` int(11) NOT NULL COMMENT '党会场id（主会场），关联表pb_party_hall字段hallId',
  `content` text NOT NULL COMMENT '党会议内容',
  `start_time` datetime NOT NULL COMMENT '开始时间',
  `end_time` datetime NOT NULL COMMENT '党会议结束时间',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='党会议';

-- ----------------------------
-- Table structure for pb_device_big_screen
-- ----------------------------
DROP TABLE IF EXISTS `pb_device_big_screen`;
CREATE TABLE `pb_device_big_screen` (
  `deviceId` int(11) NOT NULL AUTO_INCREMENT COMMENT '大屏id',
  `deviceTypeId` int(11) NOT NULL DEFAULT '19' COMMENT '设备类型id，关联表sl_base_type字段baseTypeId，type=2',
  `hallId` int(11) NOT NULL COMMENT '党会场id，关联表pb_party_hall字段hallId',
  `deviceName` varchar(50) NOT NULL COMMENT '设备名称',
  `deviceMac` varchar(30) NOT NULL COMMENT '设备mac地址',
  `deviceVersion` varchar(30) DEFAULT NULL COMMENT '设备版本号',
  `connectionStatus` int(1) NOT NULL DEFAULT '0' COMMENT '设备连接服务器状态：0离线，1在线,',
  `communicationType` int(1) NOT NULL COMMENT '通讯类型，0 Zigbee，1 WiFi，2 BLE ，3 RS485，4 NB-IOT',
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`deviceId`),
  UNIQUE KEY `deviceMacAndhouseId_Unique` (`deviceMac`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='大屏';

-- ----------------------------
-- Table structure for pb_device_ipc
-- ----------------------------
DROP TABLE IF EXISTS `pb_device_ipc`;
CREATE TABLE `pb_device_ipc` (
  `deviceId` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ipc摄像头id',
  `deviceTypeId` int(11) NOT NULL DEFAULT '15' COMMENT '设备类型id，关联表sl_base_type字段baseTypeId，type=2',
  `hallId` int(11) NOT NULL COMMENT '党会场id，关联表pb_party_hall字段hallId',
  `deviceName` varchar(50) NOT NULL COMMENT '设备名称',
  `deviceMac` varchar(30) NOT NULL COMMENT '设备mac地址',
  `deviceVersion` varchar(30) DEFAULT NULL COMMENT '设备版本号',
  `connectionStatus` int(1) NOT NULL DEFAULT '0' COMMENT '设备连接服务器状态：0离线，1在线,',
  `communicationType` int(1) NOT NULL COMMENT '通讯类型，0 Zigbee，1 WiFi，2 BLE ，3 RS485，4 NB-IOT',
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`deviceId`),
  UNIQUE KEY `deviceMacAndhouseId_Unique` (`deviceMac`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='ipc摄像头';

-- ----------------------------
-- Table structure for pb_device_outdoor_screen
-- ----------------------------
DROP TABLE IF EXISTS `pb_device_outdoor_screen`;
CREATE TABLE `pb_device_outdoor_screen` (
  `deviceId` int(11) NOT NULL AUTO_INCREMENT COMMENT '室外显屏id',
  `deviceTypeId` int(11) NOT NULL DEFAULT '16' COMMENT '设备类型id，关联表sl_base_type字段baseTypeId，type=2',
  `organizationId` int(11) NOT NULL COMMENT '党组织id，关联表pb_party_organization字段organizationId',
  `deviceName` varchar(50) NOT NULL COMMENT '设备名称',
  `deviceMac` varchar(30) NOT NULL COMMENT '设备mac地址',
  `deviceVersion` varchar(30) DEFAULT NULL COMMENT '设备版本号',
  `connectionStatus` int(1) NOT NULL DEFAULT '0' COMMENT '设备连接服务器状态：0离线，1在线,',
  `communicationType` int(1) NOT NULL COMMENT '通讯类型，0 Zigbee，1 WiFi，2 BLE ，3 RS485，4 NB-IOT',
  `lat` float(8,6) DEFAULT NULL COMMENT '纬度',
  `lng` float(9,6) DEFAULT NULL COMMENT '经度',
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`deviceId`),
  UNIQUE KEY `deviceMacAndhouseId_Unique` (`deviceMac`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='室外显屏';

-- ----------------------------
-- Table structure for pb_device_robot
-- ----------------------------
DROP TABLE IF EXISTS `pb_device_robot`;
CREATE TABLE `pb_device_robot` (
  `deviceId` int(11) NOT NULL AUTO_INCREMENT COMMENT '机器人id',
  `deviceTypeId` int(11) NOT NULL DEFAULT '18' COMMENT '设备类型id，关联表sl_base_type字段baseTypeId，type=2',
  `organizationId` int(11) NOT NULL COMMENT '党组织id，关联表pb_party_organization字段organizationId',
  `deviceName` varchar(50) NOT NULL COMMENT '设备名称',
  `deviceMac` varchar(30) NOT NULL COMMENT '设备mac地址',
  `deviceVersion` varchar(30) DEFAULT NULL COMMENT '设备版本号',
  `connectionStatus` int(1) NOT NULL DEFAULT '0' COMMENT '设备连接服务器状态：0离线，1在线,',
  `communicationType` int(1) NOT NULL COMMENT '通讯类型，0 Zigbee，1 WiFi，2 BLE ，3 RS485，4 NB-IOT',
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`deviceId`),
  UNIQUE KEY `deviceMacAndhouseId_Unique` (`deviceMac`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='机器人';

-- ----------------------------
-- Table structure for pb_device_stb
-- ----------------------------
DROP TABLE IF EXISTS `pb_device_stb`;
CREATE TABLE `pb_device_stb` (
  `deviceId` int(11) NOT NULL AUTO_INCREMENT COMMENT '机顶盒id',
  `deviceTypeId` int(11) NOT NULL DEFAULT '20' COMMENT '设备类型id，关联表pb_base_type字段baseTypeId，type=2',
  `hallId` int(11) NOT NULL COMMENT '党会场id，关联表pb_party_hall字段hallId',
  `deviceName` varchar(50) NOT NULL COMMENT '设备名称',
  `deviceMac` varchar(30) NOT NULL COMMENT '设备mac地址',
  `deviceVersion` varchar(30) DEFAULT NULL COMMENT '设备版本号',
  `connectionStatus` int(1) NOT NULL DEFAULT '0' COMMENT '设备连接服务器状态：0离线，1在线,',
  `communicationType` int(1) NOT NULL COMMENT '通讯类型，0 Zigbee，1 WiFi，2 BLE ，3 RS485，4 NB-IOT',
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `IPCuserId` varchar(256) DEFAULT NULL COMMENT 'ipc 平台id',
  `invitationCode` varchar(32) DEFAULT NULL COMMENT '验证码',
  PRIMARY KEY (`deviceId`),
  UNIQUE KEY `deviceMacAndhouseId_Unique` (`deviceMac`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='机顶盒';

-- ----------------------------
-- Table structure for pb_device_touch_screen
-- ----------------------------
DROP TABLE IF EXISTS `pb_device_touch_screen`;
CREATE TABLE `pb_device_touch_screen` (
  `deviceId` int(11) NOT NULL AUTO_INCREMENT COMMENT '触摸屏id',
  `deviceTypeId` int(11) NOT NULL DEFAULT '17' COMMENT '设备类型id，关联表sl_base_type字段baseTypeId，type=2',
  `organizationId` int(11) NOT NULL COMMENT '党组织id，关联表pb_party_organization字段organizationId',
  `deviceName` varchar(50) NOT NULL COMMENT '设备名称',
  `deviceMac` varchar(30) NOT NULL COMMENT '设备mac地址',
  `deviceVersion` varchar(30) DEFAULT NULL COMMENT '设备版本号',
  `connectionStatus` int(1) NOT NULL DEFAULT '0' COMMENT '设备连接服务器状态：0离线，1在线,',
  `communicationType` int(1) NOT NULL COMMENT '通讯类型，0 Zigbee，1 WiFi，2 BLE ，3 RS485，4 NB-IOT',
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`deviceId`),
  UNIQUE KEY `deviceMacAndhouseId_Unique` (`deviceMac`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='ipc摄像头';

-- ----------------------------
-- Table structure for pb_hall
-- ----------------------------
DROP TABLE IF EXISTS `pb_hall`;
CREATE TABLE `pb_hall` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '党会场id',
  `name` varchar(50) NOT NULL COMMENT '党会场名称',
  `province` enum('') DEFAULT NULL COMMENT '省，关联表ff_position_provice',
  `city` enum('') DEFAULT NULL COMMENT '市，关联表ff_position_city',
  `county` enum('') DEFAULT NULL COMMENT '县区，关联表ff_position_county',
  `town` enum('') DEFAULT NULL COMMENT '街道，关联表ff_position_town',
  `detail` varchar(100) DEFAULT NULL COMMENT '场所详细地址',
  `lat` decimal(8,6) DEFAULT NULL COMMENT '纬度',
  `lng` decimal(9,6) DEFAULT NULL COMMENT '经度',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='党会场';

-- ----------------------------
-- Table structure for pb_notice
-- ----------------------------
DROP TABLE IF EXISTS `pb_notice`;
CREATE TABLE `pb_notice` (
  `noticeId` int(11) NOT NULL AUTO_INCREMENT,
  `noticeName` varchar(255) NOT NULL COMMENT '公告名称',
  `noticeContent` text NOT NULL COMMENT '公告内容',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `organizationId` int(11) DEFAULT NULL COMMENT '组织名称',
  `image` varchar(255) NOT NULL COMMENT '图片',
  `summary` varchar(1000) DEFAULT NULL COMMENT '摘要',
  PRIMARY KEY (`noticeId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for pb_party_activity_center
-- ----------------------------
DROP TABLE IF EXISTS `pb_party_activity_center`;
CREATE TABLE `pb_party_activity_center` (
  `activityCenterId` int(11) NOT NULL AUTO_INCREMENT COMMENT '党活动中心id',
  `organizationId` int(11) NOT NULL COMMENT '党组织id，关联表pb_party_organization字段organizationId',
  `activityCenterName` varchar(50) NOT NULL COMMENT '党活动中心名称',
  `province` int(3) DEFAULT NULL COMMENT '省，关联表position_provice',
  `city` bigint(12) DEFAULT NULL COMMENT '市，关联表position_city',
  `county` bigint(12) DEFAULT NULL COMMENT '县区，关联表position_county',
  `town` bigint(12) DEFAULT NULL COMMENT '街道，关联表position_town',
  `address` varchar(100) DEFAULT NULL COMMENT '场所详细地址',
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `qq` varchar(32) NOT NULL DEFAULT '' COMMENT 'qq群',
  `telephone` varchar(32) NOT NULL DEFAULT '12345678910' COMMENT '电话联系方式',
  `imageUrl` varchar(255) DEFAULT NULL COMMENT '简介图片地址',
  `introduction` text COMMENT '园区党建',
  `centerIntroduction` text COMMENT '活动中心简介',
  `townIntroduction` text COMMENT '地区简介',
  `schoolIntroduction` text COMMENT '党校简介',
  `areaLatLng` text COMMENT '地图某区域经纬度，格式经纬度用英文逗号分隔',
  `markerList` varchar(255) DEFAULT NULL COMMENT '区域内标记的点坐标',
  PRIMARY KEY (`activityCenterId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='党活动中心';

-- ----------------------------
-- Table structure for pb_party_meet_hall
-- ----------------------------
DROP TABLE IF EXISTS `pb_party_meet_hall`;
CREATE TABLE `pb_party_meet_hall` (
  `meetHallId` int(11) NOT NULL AUTO_INCREMENT COMMENT '党会议与分党会场id',
  `meetId` int(11) NOT NULL COMMENT '党会议id，关联表pb_party_meet字段meetId',
  `hallId` int(11) NOT NULL COMMENT '党会场id（分会场），关联表pb_party_hall字段hallId',
  PRIMARY KEY (`meetHallId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='党会议与分党会场';

-- ----------------------------
-- Table structure for pb_party_meet_member
-- ----------------------------
DROP TABLE IF EXISTS `pb_party_meet_member`;
CREATE TABLE `pb_party_meet_member` (
  `meetHallId` int(11) NOT NULL AUTO_INCREMENT COMMENT '党会议与党员id',
  `meetId` int(11) NOT NULL COMMENT '党会议id，关联表pb_party_meet字段meetId',
  `memberId` int(11) NOT NULL COMMENT '党员id，关联表pb_party_member字段memberId',
  PRIMARY KEY (`meetHallId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='党会议与党员';

-- ----------------------------
-- Table structure for pb_party_member
-- ----------------------------
DROP TABLE IF EXISTS `pb_party_member`;
CREATE TABLE `pb_party_member` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '党员id',
  `organization_id` int(11) NOT NULL COMMENT '党组织id，关联表pb_party_organization字段organizationId',
  `userId` varchar(100) DEFAULT NULL COMMENT '用户id，关联表sys_user字段USER_ID，用app中判断是否管理员，null不是管理员',
  `name` varchar(50) NOT NULL COMMENT '党员姓名',
  `card_no` varchar(20) NOT NULL COMMENT '证件编号',
  `sex` int(1) NOT NULL COMMENT '性别, 0女，1男，2不选择',
  `role` int(2) DEFAULT NULL COMMENT '党级别',
  `native_place` varchar(50) DEFAULT NULL COMMENT '籍贯',
  `nation` varchar(50) DEFAULT NULL COMMENT '民族',
  `mobile` varchar(64) DEFAULT NULL COMMENT '手机号码',
  `partyPosition` varchar(128) DEFAULT NULL COMMENT '党内职务',
  `joinTime` datetime DEFAULT NULL COMMENT '入党时间',
  `birth_time` date DEFAULT NULL COMMENT '出生年月日',
  `create_time` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='党员';

-- ----------------------------
-- Table structure for pb_party_organization
-- ----------------------------
DROP TABLE IF EXISTS `pb_party_organization`;
CREATE TABLE `pb_party_organization` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '党组织id',
  `superOrganizationId` int(11) NOT NULL DEFAULT '0' COMMENT '上级党组织id，没有上级默认为0，关联表pb_party_organization字段organizationId',
  `name` varchar(50) NOT NULL COMMENT '党组织名称',
  `user_id` varchar(100) DEFAULT NULL COMMENT '管理员用户id，关联表sys_user字段USER_ID',
  `lat` float(8,6) DEFAULT NULL COMMENT '纬度',
  `lng` float(9,6) DEFAULT NULL COMMENT '经度',
  `create_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `introduction` text COMMENT '党组织概况',
  `imageUrl` varchar(255) DEFAULT NULL COMMENT '党组织概况图片',
  `province` int(3) DEFAULT NULL COMMENT '省，关联表ff_position_provice',
  `city` bigint(12) DEFAULT NULL COMMENT '市，关联表ff_position_city',
  `county` bigint(12) DEFAULT NULL COMMENT '县区，关联表ff_position_county',
  `town` bigint(12) DEFAULT NULL COMMENT '街道，关联表ff_position_town',
  `address` varchar(100) DEFAULT NULL COMMENT '组织详细地址',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='党组织';

-- ----------------------------
-- Table structure for pb_propaganda
-- ----------------------------
DROP TABLE IF EXISTS `pb_propaganda`;
CREATE TABLE `pb_propaganda` (
  `propagandaId` int(11) NOT NULL AUTO_INCREMENT COMMENT '宣传id',
  `organizationId` int(11) NOT NULL COMMENT '党组织id，关联表pb_party_organization字段organizationId',
  `propagandaPlateId` int(11) DEFAULT NULL COMMENT '宣传板块id，可以不选择，关联表pb_propaganda_plate字段propagandaPlateId',
  `title` varchar(50) NOT NULL COMMENT '标题',
  `content` text COMMENT '宣传内容，json格式，type=1格式：{}',
  `type` int(2) NOT NULL COMMENT '宣传类型，1人物描述，2图文宣传，3纯图宣传，4视频宣传，5紧急通知宣传',
  `startTime` datetime NOT NULL COMMENT '开始时间',
  `endTime` datetime NOT NULL COMMENT '结束时间',
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `isAd` int(1) NOT NULL DEFAULT '0' COMMENT '是否是广告，0否，1是',
  `isRelease` int(1) NOT NULL DEFAULT '0',
  `summary` varchar(1000) DEFAULT NULL COMMENT '摘要',
  PRIMARY KEY (`propagandaId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='党组织';

-- ----------------------------
-- Table structure for pb_propaganda_outdoor_screen
-- ----------------------------
DROP TABLE IF EXISTS `pb_propaganda_outdoor_screen`;
CREATE TABLE `pb_propaganda_outdoor_screen` (
  `propagandaOutdoorScreenId` int(11) NOT NULL AUTO_INCREMENT COMMENT '宣传与室外显屏id',
  `propagandaId` int(11) NOT NULL COMMENT '宣传id，关联表pb_propaganda字段propagandaId',
  `deviceId` int(11) NOT NULL COMMENT '室外显屏id，关联表pb_device_outdoor_screen字段deviceId',
  PRIMARY KEY (`propagandaOutdoorScreenId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='宣传与室外显屏';

-- ----------------------------
-- Table structure for pb_propaganda_plate
-- ----------------------------
DROP TABLE IF EXISTS `pb_propaganda_plate`;
CREATE TABLE `pb_propaganda_plate` (
  `propagandaPlateId` int(11) NOT NULL AUTO_INCREMENT COMMENT '宣传板块id',
  `organizationId` int(11) NOT NULL COMMENT '党组织id，关联表pb_party_organization字段organizationId',
  `propagandaPlateName` varchar(50) NOT NULL COMMENT '宣传板块名称',
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`propagandaPlateId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='党组织';

-- ----------------------------
-- Table structure for pb_propaganda_robot
-- ----------------------------
DROP TABLE IF EXISTS `pb_propaganda_robot`;
CREATE TABLE `pb_propaganda_robot` (
  `propagandaRobotId` int(11) NOT NULL AUTO_INCREMENT COMMENT '宣传与机器人id',
  `propagandaId` int(11) NOT NULL COMMENT '宣传id，关联表pb_propaganda字段propagandaId',
  `deviceId` int(11) NOT NULL COMMENT '机器人id，关联表pb_device_robot字段deviceId',
  PRIMARY KEY (`propagandaRobotId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='宣传与机器人';

-- ----------------------------
-- Table structure for pb_propaganda_stb
-- ----------------------------
DROP TABLE IF EXISTS `pb_propaganda_stb`;
CREATE TABLE `pb_propaganda_stb` (
  `propagandaStbId` int(11) NOT NULL COMMENT '宣传与机顶盒id',
  `propagandaId` int(11) NOT NULL COMMENT '宣传id，关联表pb_propaganda字段propagandaId',
  `deviceId` int(11) NOT NULL COMMENT '机顶盒id，关联表pb_device_stb字段deviceId',
  PRIMARY KEY (`propagandaStbId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for pb_propaganda_touch_screen
-- ----------------------------
DROP TABLE IF EXISTS `pb_propaganda_touch_screen`;
CREATE TABLE `pb_propaganda_touch_screen` (
  `propagandaTouchScreenId` int(11) NOT NULL AUTO_INCREMENT COMMENT '宣传与触摸屏id',
  `propagandaId` int(11) NOT NULL COMMENT '宣传id，关联表pb_propaganda字段propagandaId',
  `deviceId` int(11) NOT NULL COMMENT '触摸屏id，关联表pb_device_touch_screen字段deviceId',
  PRIMARY KEY (`propagandaTouchScreenId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='宣传与触摸屏';

-- ----------------------------
-- Table structure for position_city
-- ----------------------------
DROP TABLE IF EXISTS `position_city`;
CREATE TABLE `position_city` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `province_id` int(10) unsigned NOT NULL COMMENT '地级市id',
  `city_id` bigint(20) unsigned NOT NULL COMMENT '县级市id',
  `city_name` char(64) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `city_id` (`city_id`),
  KEY `province_id` (`province_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='县级市数据库';

-- ----------------------------
-- Table structure for position_county
-- ----------------------------
DROP TABLE IF EXISTS `position_county`;
CREATE TABLE `position_county` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '地级市主键ID',
  `city_id` bigint(20) unsigned NOT NULL COMMENT '地级市id',
  `county_id` bigint(20) unsigned NOT NULL COMMENT '县级id',
  `county_name` char(64) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `county_id` (`county_id`),
  KEY `city_id` (`city_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='地区市数据库';

-- ----------------------------
-- Table structure for position_provice
-- ----------------------------
DROP TABLE IF EXISTS `position_provice`;
CREATE TABLE `position_provice` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `provice_id` int(11) unsigned NOT NULL COMMENT '省份id、省份编号',
  `provice_name` char(32) NOT NULL COMMENT '省份名称',
  PRIMARY KEY (`id`),
  UNIQUE KEY `provice_id` (`provice_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='省份数据库';

-- ----------------------------
-- Table structure for position_town
-- ----------------------------
DROP TABLE IF EXISTS `position_town`;
CREATE TABLE `position_town` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `county_id` bigint(20) unsigned NOT NULL COMMENT '县级市id',
  `town_id` bigint(20) unsigned NOT NULL COMMENT '镇id',
  `town_name` char(64) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `town_id` (`town_id`),
  KEY `county_id` (`county_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='镇数据库';

-- ----------------------------
-- Table structure for schedule
-- ----------------------------
DROP TABLE IF EXISTS `schedule`;
CREATE TABLE `schedule` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `airendtime` datetime DEFAULT NULL,
  `airstarttime` datetime DEFAULT NULL,
  `channelcode` varchar(255) DEFAULT NULL,
  `channelid` int(11) DEFAULT '0',
  `cpcontentid` varchar(255) DEFAULT NULL,
  `cpspid` int(11) DEFAULT '1',
  `deletetime` datetime DEFAULT NULL,
  `description` varchar(4096) DEFAULT NULL,
  `duration` varchar(255) DEFAULT NULL,
  `objectid` varchar(255) DEFAULT NULL,
  `programname` varchar(255) DEFAULT NULL,
  `releasestatus` int(11) DEFAULT '0',
  `releasetime` datetime DEFAULT NULL,
  `startdate` datetime DEFAULT NULL,
  `starttime` varchar(255) DEFAULT NULL,
  `status` int(11) DEFAULT NULL,
  `storageduration` varchar(255) DEFAULT NULL,
  `replay` int(11) DEFAULT '0',
  `language` varchar(8) DEFAULT NULL,
  `satellite` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `channelid` (`channelid`),
  KEY `satellite` (`satellite`)
) ENGINE=InnoDB AUTO_INCREMENT=84068584 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for sys_gl_qx
-- ----------------------------
DROP TABLE IF EXISTS `sys_gl_qx`;
CREATE TABLE `sys_gl_qx` (
  `GL_ID` varchar(100) NOT NULL,
  `ROLE_ID` varchar(100) DEFAULT NULL,
  `FX_QX` int(10) DEFAULT NULL,
  `FW_QX` int(10) DEFAULT NULL,
  `QX1` int(10) DEFAULT NULL,
  `QX2` int(10) DEFAULT NULL,
  `QX3` int(10) DEFAULT NULL,
  `QX4` int(10) DEFAULT NULL,
  PRIMARY KEY (`GL_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for sys_logs
-- ----------------------------
DROP TABLE IF EXISTS `sys_logs`;
CREATE TABLE `sys_logs` (
  `rid` int(11) NOT NULL AUTO_INCREMENT,
  `userId` varchar(32) DEFAULT NULL,
  `userName` varchar(100) DEFAULT NULL,
  `operId` int(11) DEFAULT NULL COMMENT '运营公司ID',
  `type` varchar(3) DEFAULT NULL COMMENT '类型，APP, HXC,MKJ,SNJ',
  `platform` char(1) DEFAULT NULL COMMENT '推送平台，0：anroid，1：ios',
  `appType` char(1) DEFAULT NULL COMMENT '类型，0：门口机，1：室内机,2:手机用户',
  `intfName` varchar(50) DEFAULT NULL COMMENT '接口名称',
  `creTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '时间',
  `content` varchar(500) DEFAULT NULL COMMENT '内容',
  PRIMARY KEY (`rid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for sys_menu
-- ----------------------------
DROP TABLE IF EXISTS `sys_menu`;
CREATE TABLE `sys_menu` (
  `MENU_ID` int(11) NOT NULL,
  `MENU_NAME` varchar(255) DEFAULT NULL,
  `MENU_URL` varchar(255) DEFAULT NULL,
  `PARENT_ID` varchar(100) DEFAULT NULL,
  `MENU_ORDER` varchar(100) DEFAULT NULL,
  `MENU_ICON` varchar(30) DEFAULT NULL,
  `MENU_TYPE` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`MENU_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for sys_oper_logs
-- ----------------------------
DROP TABLE IF EXISTS `sys_oper_logs`;
CREATE TABLE `sys_oper_logs` (
  `rid` int(11) NOT NULL AUTO_INCREMENT,
  `operId` int(11) DEFAULT NULL COMMENT '运营公司ID',
  `type` varchar(3) DEFAULT NULL COMMENT '类型，APP, HXC,MKJ,SNJ',
  `platform` char(1) DEFAULT NULL COMMENT '推送平台，0：anroid，1：ios',
  `appType` char(1) DEFAULT NULL COMMENT '类型，0：门口机，1：室内机,2:手机用户',
  `intfName` varchar(50) DEFAULT NULL COMMENT '动作名称',
  `creTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '时间',
  `content` varchar(500) DEFAULT NULL COMMENT '内容',
  PRIMARY KEY (`rid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for sys_role
-- ----------------------------
DROP TABLE IF EXISTS `sys_role`;
CREATE TABLE `sys_role` (
  `ROLE_ID` varchar(100) NOT NULL,
  `ROLE_NAME` varchar(100) DEFAULT NULL,
  `RIGHTS` varchar(255) DEFAULT NULL,
  `PARENT_ID` varchar(100) DEFAULT NULL,
  `ADD_QX` varchar(255) DEFAULT NULL,
  `DEL_QX` varchar(255) DEFAULT NULL,
  `EDIT_QX` varchar(255) DEFAULT NULL,
  `CHA_QX` varchar(255) DEFAULT NULL,
  `QX_ID` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ROLE_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for sys_user
-- ----------------------------
DROP TABLE IF EXISTS `sys_user`;
CREATE TABLE `sys_user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(255) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL,
  `NAME` varchar(255) DEFAULT NULL,
  `RIGHTS` varchar(255) DEFAULT NULL,
  `ROLE_ID` varchar(100) DEFAULT NULL,
  `LAST_LOGIN` varchar(255) DEFAULT NULL,
  `IP` varchar(100) DEFAULT NULL,
  `STATUS` varchar(32) DEFAULT NULL,
  `BZ` varchar(255) DEFAULT NULL,
  `SKIN` varchar(100) DEFAULT NULL,
  `EMAIL` varchar(32) DEFAULT NULL,
  `NUMBER` varchar(100) DEFAULT NULL,
  `PHONE` varchar(32) DEFAULT NULL,
  `cardNo` char(20) DEFAULT NULL COMMENT '证件编号',
  `sex` int(1) DEFAULT '2' COMMENT '性别, 0女，1男，2不选择',
  `lat` float(8,6) DEFAULT NULL COMMENT '纬度',
  `lng` float(9,6) DEFAULT NULL COMMENT '经度',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for sys_user_qx
-- ----------------------------
DROP TABLE IF EXISTS `sys_user_qx`;
CREATE TABLE `sys_user_qx` (
  `U_ID` varchar(100) NOT NULL,
  `C1` int(10) DEFAULT NULL,
  `C2` int(10) DEFAULT NULL,
  `C3` int(10) DEFAULT NULL,
  `C4` int(10) DEFAULT NULL,
  `Q1` int(10) DEFAULT NULL,
  `Q2` int(10) DEFAULT NULL,
  `Q3` int(10) DEFAULT NULL,
  `Q4` int(10) DEFAULT NULL,
  PRIMARY KEY (`U_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
