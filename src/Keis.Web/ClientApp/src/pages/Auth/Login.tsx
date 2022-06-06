import { UserOutlined } from "@ant-design/icons";
import { Alert, Button, Checkbox, Form, Input, Layout, Modal, Timeline } from "antd";
import { FC, useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { PageView } from "../../components";
import { useAuth } from "../../context";

const { Header, Content } = Layout;

interface ILogin {
  from?: string;
}

interface ILoginForm {
  username: string;
  password: string;
  remember: boolean;
}

const Login: FC<ILogin> = () => {
  const { signIn } = useAuth();
  const [form] = Form.useForm();
  const [loginFailed, setLoginFailed] = useState(false);
  const [loginErrors, setLoginErrors] = useState<string[]>();
  const navigate = useNavigate();
  const [returnUrl, setReturnUrl] = useState("/");
  const [showForget, setShowForget] = useState(false);
  const { state } = useLocation();
  const _state: any = state;

  useEffect(() => {
    document.title = "Login";

    if (_state && _state.from && _state.from.pathname) {
      setReturnUrl(_state.from.pathname);
    }
  }, [_state]);

  const onFinish = async ({
    username,
    password,
    remember,
  }: ILoginForm): Promise<void> => {
    const result = await signIn({ username, password, remember });
    if (result !== null && result.ok) {
      navigate(returnUrl, { replace: true });
    } else {
      // login failed do something
      setLoginFailed(true);
      setLoginErrors(result?.errors);
    }
  };

  const onRecover = () => {}

  return (
    <Layout className="h-screen">
      <Header
        style={{
          backgroundColor: "white",
          borderBottom: "1px solid rgba(128, 128, 128, 0.3)",
        }}
      >
        Keis Desk
      </Header>
      <Content className="bg-white">
        <div className="relative w-full h-full flex-auto px-3 md:px-16 lg:px-36 py-8">
          <PageView title="">
            <div className="flex w-full items-center pb-5">
              <div className="flex w-1/2"></div>
              <div style={{ width: "640px" }}>
                <div className="font-bold text-2xl pb-5">Login</div>
                {loginFailed &&
                  loginErrors?.map((e) => (
                    <Alert key={e} message={e} type="error" className="py-2" />
                  ))}
                <Form form={form} onFinish={onFinish} layout="vertical">
                  <Form.Item name="username" label="Username or Email">
                    <Input />
                  </Form.Item>
                  <Form.Item name="password" label="Password">
                    <Input.Password />
                  </Form.Item>
                  <Form.Item name="remember" valuePropName="checked">
                    <Checkbox>Remember me</Checkbox>
                    <span className="pl-2">|</span>
                    <Button type="link" onClick={() => setShowForget(true)}>
                      <span className="text-xs">Forget Password?</span>
                    </Button>
                  </Form.Item>
                  <Form.Item>
                    <Button type="primary" htmlType="submit">
                      Login
                    </Button>
                  </Form.Item>
                </Form>
              </div>
              <div className="flex w-1/2"></div>
            </div>
          </PageView>
        </div>
        <Modal
          visible={showForget}
          title="Get back into your account"
          onCancel={() => setShowForget(false)}
          destroyOnClose={true}
          footer={null}
        >
          <Form layout="vertical" onFinish={onRecover}>
        <Timeline>
          <Timeline.Item dot={<UserOutlined style={{ fontSize: "16px" }} />}>
            <div className="font-bold mb-2">Who are you?</div>
            <div className="pb-3">To recover your account, begin by entering your email or username</div>
            <Form.Item name="username" label="Username or Email">
             <Input />
            </Form.Item>
          </Timeline.Item>
          <Timeline.Item>
            <Form.Item>
              <Button type="primary" htmlType="submit">
                Recover
              </Button>
            </Form.Item>
          </Timeline.Item>
        </Timeline>
      </Form>
        </Modal>
      </Content>
    </Layout>
  );
};

export default Login;
