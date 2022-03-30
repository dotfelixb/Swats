import { Button, Checkbox, Form, Input, Layout } from "antd";
import { FC } from "react";
import { useNavigate } from "react-router-dom";
import { PageView } from "../../components";
import { useAuth } from "../../context";

const { Header, Content } = Layout;
interface ILogin {}

const Login: FC<ILogin> = () => {
  const {signIn} = useAuth();
  const [form] = Form.useForm();
  const navigate = useNavigate();

  const onFinish = async ({username, password, remember} : {username: string, password: string, remember: boolean}): Promise<void> => {
    const result = await signIn({username, password, remember});
    if(result){
      console.log(result);
      
      navigate("/");
    }
    // login failed do something
  }

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
                <Form form={form} onFinish={onFinish} layout="vertical">
                  <Form.Item name="username" label="Username or Email">
                    <Input placeholder="" />
                  </Form.Item>
                  <Form.Item name="password" label="Password">
                    <Input.Password placeholder="" />
                  </Form.Item>
                  <Form.Item
                    name="remember"
                    valuePropName="checked" 
                  >
                    <Checkbox>Remember me</Checkbox>
                  </Form.Item>
                  <Form.Item>
                    <Button type="primary" htmlType="submit">Login</Button>
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
