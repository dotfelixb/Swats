import { FC, useEffect, useState } from "react";
import { Avatar, Button, Dropdown, Layout, Menu, MenuProps } from "antd";
import {
  BellOutlined,
  CustomerServiceOutlined,
  DashboardOutlined,
  SettingOutlined,
  TagsOutlined,
} from "@ant-design/icons";
import { Link, Outlet } from "react-router-dom";
import { RequireAuth } from ".";
import { useAuth } from "../context";

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
  const { user, signOut } = useAuth();
  const [loggedIn, setLoggedIn] = useState<string>();

  useEffect(() => {
    setLoggedIn(user?.fullname?.substring(0, 1) ?? "U");
  }, [user]);

  const menu = (
    <Menu
      items={[
        {
          key: "1",
          label: (
            <a rel="noopener noreferrer" href="/login" onClick={signOut}>
              Log out
            </a>
          ),
        },
      ]}
    />
  );

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
        <div className="flex w-full items-center pb-5">
          <div className="flex w-1/2 font-bold ">Keis Desk</div>
          <div className="flex w-1/2 space-x-2 items-center justify-end">
              <Button shape="circle" icon={<BellOutlined />} />
              <Dropdown overlay={menu} placement="bottom" arrow>
                <Avatar
                  style={{
                    color: "#fff",
                    backgroundColor: "#6366F1",
                    cursor: "pointer",
                  }}
                >
                  {loggedIn}
                </Avatar>
              </Dropdown>
          </div>
        </div>
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
