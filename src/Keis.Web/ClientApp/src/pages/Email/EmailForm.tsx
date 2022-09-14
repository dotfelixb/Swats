import {
  CommentOutlined,
  FormOutlined,
  LeftSquareOutlined,
  RightSquareOutlined,
} from "@ant-design/icons";
import { Alert, Button, Form, Input, Select, Timeline } from "antd";
import { FC } from "react";
import { IFetchEmail } from "../../interfaces";

const { TextArea } = Input;

interface IEmailForm {
  hasFormErrors: boolean;
  formErrors: string[];
  email?: IFetchEmail;
  onFinish: (values: any) => void;
}

const EmailForm: FC<IEmailForm> = ({
  hasFormErrors,
  formErrors,
  email,
  onFinish,
}) => (
  <Form layout="vertical" onFinish={onFinish}>
    {hasFormErrors &&
      formErrors?.map((e) => (
        <div key={e} className="py-2">
          <Alert message={e} type="error" className="py-2" />
        </div>
      ))}
    <Timeline>
      <Timeline.Item dot={<FormOutlined style={{ fontSize: "16px" }} />}>
        <div className="font-bold mb-2">Email information</div>
        <div className="grid grid-cols-2 gap-3">
          <Form.Item name="address" label="Email Address" initialValue={email?.address}>
            <Input />
          </Form.Item>
          <Form.Item name="name" label="Display Name" initialValue={email?.name}>
            <Input />
          </Form.Item>
        </div>
        <div className="grid grid-cols-2 gap-3">
          <Form.Item name="username" label="Username" initialValue={email?.username}>
            <Input autoComplete="new-email" />
          </Form.Item>
          <Form.Item name="password" label="Password" initialValue={email?.password}>
            <Input.Password autoComplete="new-password" />
          </Form.Item>
        </div>
      </Timeline.Item>
      <Timeline.Item dot={<LeftSquareOutlined style={{ fontSize: "16px" }} />}>
        <div className="font-bold mb-2">Incoming mail server settings</div>
        <Form.Item name="inhost" label="Host Name" initialValue={email?.inHost}>
          <Input />
        </Form.Item>
        <div className="grid grid-cols-3 gap-3">
          <Form.Item name="inprotocol" label="Protocol" initialValue={email?.inProtocol}>
            <Select>
              <Select.Option value="1">IMAP</Select.Option>
              <Select.Option value="2">POP</Select.Option>
              <Select.Option value="3">SMTP</Select.Option>
              <Select.Option value="4">Exchange Web Service</Select.Option>
            </Select>
          </Form.Item>
          <Form.Item name="inport" label="Port" initialValue={email?.inPort}>
            <Input />
          </Form.Item>
          <Form.Item name="insecurity" label="Encryption" initialValue={email?.inSecurity}>
            <Select>
              <Select.Option value="1">None</Select.Option>
              <Select.Option value="2">SSL</Select.Option>
              <Select.Option value="3">TLS</Select.Option>
            </Select>
          </Form.Item>
        </div>
      </Timeline.Item>
      <Timeline.Item dot={<RightSquareOutlined style={{ fontSize: "16px" }} />}>
        <div className="font-bold mb-2">Outgoing mail server settings</div>
        <Form.Item name="outhost" label="Host Name" initialValue={email?.outHost}>
          <Input />
        </Form.Item>
        <div className="grid grid-cols-3 gap-3">
          <Form.Item name="outprotocol" label="Protocol" initialValue={email?.outProtocol}>
            <Select>
              <Select.Option value="1">IMAP</Select.Option>
              <Select.Option value="2">POP3</Select.Option>
              <Select.Option value="3">SMTP</Select.Option>
              <Select.Option value="4">Exchange Web Service</Select.Option>
            </Select>
          </Form.Item>
          <Form.Item name="outport" label="Port" initialValue={email?.outPort}>
            <Input />
          </Form.Item>
          <Form.Item name="outsecurity" label="Encryption" initialValue={email?.outSecurity}>
            <Select>
              <Select.Option value="1">None</Select.Option>
              <Select.Option value="2">SSL</Select.Option>
              <Select.Option value="3">TLS</Select.Option>
            </Select>
          </Form.Item>
        </div>
      </Timeline.Item>
      <Timeline.Item dot={<CommentOutlined style={{ fontSize: "16px" }} />}>
        <div className="font-bold mb-2">
          Addition note for this tag (optional)
        </div>
        <Form.Item name="status" label="Status" initialValue={email?.status}>
          <Select>
            <Select.Option value="1">Active</Select.Option>
            <Select.Option value="2">Inactive</Select.Option>
          </Select>
        </Form.Item>
        <Form.Item name="note" label="Note" initialValue={email?.note}>
          <TextArea rows={4} />
        </Form.Item>
      </Timeline.Item>
      <Timeline.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit">
            Submit
          </Button>
        </Form.Item>
      </Timeline.Item>
    </Timeline>
  </Form>
);

export default EmailForm;
