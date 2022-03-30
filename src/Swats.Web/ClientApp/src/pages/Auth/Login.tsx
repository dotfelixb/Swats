import { InfoCircleOutlined } from "@ant-design/icons";
import { Button, Checkbox, Form, Input, Layout } from "antd";
import React, { FC } from "react";
import { PageView } from "../../components";

const { Header, Content } = Layout;
interface ILogin {}

const Login: FC<ILogin> = () => {
  const [form] = Form.useForm();

  return (
    <Layout className="h-screen">
      <Header
        style={{
          backgroundColor: "white",
          borderBottom: "1px solid rgba(128, 128, 128, 0.3)",
        }}
      >
        SWATS
      </Header>
      <Content className="bg-white">
        <div className="relative w-full h-full flex-auto px-3 md:px-16 lg:px-36 py-8">
          <PageView title="Login">
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
              <div>
                <Form form={form} layout="vertical">
                  <Form.Item label="Username or Email">
                    <Input placeholder="" />
                  </Form.Item>
                  <Form.Item label="Password">
                    <Input.Password placeholder="" />
                  </Form.Item>
                  <Form.Item
                    name="remember"
                    valuePropName="checked" 
                  >
                    <Checkbox>Remember me</Checkbox>
                  </Form.Item>
                  <Form.Item>
                    <Button type="primary">Login</Button>
                  </Form.Item>
                </Form>
              </div>
              <div></div>
            </div>
          </PageView>
        </div>
      </Content>
    </Layout>
  );
};

export default Login;
