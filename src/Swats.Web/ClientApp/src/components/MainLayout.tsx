import React, { FC } from "react";
import { Layout, Menu } from "antd";
import {
  DashboardOutlined,
  PhoneOutlined,
  SettingOutlined,
  TagsOutlined,
} from "@ant-design/icons";
import { Link } from "react-router-dom";

const { Header, Sider, Content } = Layout;

const navStyle = { display: "flex", justifyContent: "center" };

interface IMainLayout {}

const MainLayout: FC<IMainLayout> = ({ children }) => {
  return (
    <Layout style={{ height: "100vh" }}>
      <Header
        style={{
          backgroundColor: "white",
          borderBottom: "1px solid rgba(128, 128, 128, 0.3)",
        }}
      >
        SWATS
      </Header>

      <Layout>
        <Sider
          width={64}
          theme="light"
          style={{ borderRight: "1px solid rgba(128, 128, 128, 0.3)" }}
        >
          <Menu style={{ borderRight: "none" }}>
            <Menu.Item key={"Dashboard"} style={navStyle}>
              <Link to="/">
                <DashboardOutlined />
              </Link>
            </Menu.Item>

            <Menu.Item key={"Ticket"} style={navStyle}>
              <Link to="/ticket">
                <TagsOutlined />
              </Link>
            </Menu.Item>

            <Menu.Item key={"Agent"} style={navStyle}>
              <Link to="/agent">
                <PhoneOutlined />
              </Link>
            </Menu.Item>

            <Menu.Item key={"Settings"} style={navStyle}>
              <Link to="/settings">
                <SettingOutlined />
              </Link>
            </Menu.Item>
          </Menu>
        </Sider>

        <Content style={{ backgroundColor: "white" }}>{children}</Content>
      </Layout>
    </Layout>
  );
};

export default MainLayout;
