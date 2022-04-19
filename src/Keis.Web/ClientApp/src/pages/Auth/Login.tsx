import { Alert, Button, Checkbox, Form, Input, Layout } from "antd";
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
    const { state } = useLocation();
    const _state: any = state;

    useEffect(() => {
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
                    <PageView title="Login">
                        <div className="grid grid-cols-1 lg:grid-cols-2 gap-3">
                            <div>
                                {loginFailed &&
                                    loginErrors?.map((e) => (
                                        <Alert key={e} message={e} type="error" className="py-2" />
                                    ))}
                                <Form form={form} onFinish={onFinish} layout="vertical">
                                    <Form.Item name="username" label="Username or Email">
                                        <Input placeholder="" />
                                    </Form.Item>
                                    <Form.Item name="password" label="Password">
                                        <Input.Password placeholder="" />
                                    </Form.Item>
                                    <Form.Item name="remember" valuePropName="checked">
                                        <Checkbox>Remember me</Checkbox>
                                    </Form.Item>
                                    <Form.Item>
                                        <Button type="primary" htmlType="submit">
                                            Login
                                        </Button>
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