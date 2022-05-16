import React, { FC } from "react";
import { Layout, Menu, MenuProps } from "antd";
import {
  CustomerServiceOutlined,
  DashboardOutlined,
  SettingOutlined,
  TagsOutlined,
} from "@ant-design/icons";
import { Link, Outlet } from "react-router-dom";
import { RequireAuth } from ".";

const { Header, Sider, Content } = Layout;

const items: MenuProps["items"] = [
  {
    key: "dashboard",
    label: (
      <Link to="/">
        <DashboardOutlined />
      </Link>
    ),
  },
  {
    key: "ticket",
    label: (
      <Link to="/ticket">
        <TagsOutlined />
      </Link>
    ),
  },
  {
    key: "agent",
    label: (
      <Link to="/agent">
        <CustomerServiceOutlined />
      </Link>
    ),
  },
  {
    key: "admin",
    label: (
      <Link to="/admin">
        <SettingOutlined />
      </Link>
    ),
  },
];

interface IMainLayout {}

const MainLayout: FC<IMainLayout> = ({ children }) => {
  return (
    <Layout style={{ height: "100vh" }}>
      <Header
        style={{
          backgroundColor: "white",
          height: "48px",
          lineHeight: "48px",
          borderBottom: "1px solid rgba(128, 128, 128, 0.3)",
        }}
      >
        Keis Desk
      </Header>

      <Layout>
        <Sider
          width={64}
          theme="light"
          style={{ borderRight: "1px solid rgba(128, 128, 128, 0.3)" }}
        >
          <Menu mode="inline" items={items} />
        </Sider>

        <Content style={{ backgroundColor: "white", overflowY: "scroll" }}>
          <div className="relative w-full flex-auto px-3 md:px-16 lg:px-36 py-8">
            <RequireAuth>
              <>
                {children}
                <Outlet />
              </>
            </RequireAuth>
          </div>
        </Content>
      </Layout>
    </Layout>
  );
};

export default MainLayout;
