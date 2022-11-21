import { CommentOutlined, FormOutlined } from "@ant-design/icons";
import { Alert, Button, Form, Input, Select, Timeline } from "antd";
import { FC } from "react";
import { IFetchTemplate } from "../../interfaces";

const { TextArea } = Input;
interface ITemplateForm {
  hasFormErrors: boolean;
  formErrors: string[];
  template?: IFetchTemplate;
  mergeList: string[];
  onFinish: (values: any) => void;
}

const TemplateForm: FC<ITemplateForm> = ({
  hasFormErrors,
  formErrors,
  template,
  mergeList,
  onFinish,
}) => {
  return (
    <Form layout="vertical" onFinish={onFinish}>
      {hasFormErrors &&
        formErrors?.map((e) => (
          <div key={e} className="py-2">
            <Alert message={e} type="error" className="py-2" />
          </div>
        ))}
      <Timeline>
        <Timeline.Item dot={<FormOutlined style={{ fontSize: "16px" }} />}>
          <div className="font-bold mb-2">Template name and type</div>
          <Form.Item name="name" label="Name" initialValue={template?.name}>
            <Input />
          </Form.Item>
          <Form.Item name="mergetags" label="Merge Tags" initialValue={template?.mergeTags}>
            <Select showSearch mode="multiple">
             {mergeList.map(e => (
              <Select.Option key={e} value={e}>{e}</Select.Option>
             ))}
            </Select>
          </Form.Item>
          <Form.Item name="status" label="Status" initialValue={template?.status}>
            <Select>
              <Select.Option value="1">Active</Select.Option>
              <Select.Option value="2">Inactive</Select.Option>
            </Select>
          </Form.Item>
        </Timeline.Item>
        <Timeline.Item dot={<CommentOutlined style={{ fontSize: "16px" }} />}>
          <div className="font-bold mb-2">
            Template subject and body
          </div>
          <Form.Item name="subject" label="Subject" initialValue={template?.subject}>
            <Input />
          </Form.Item>
          <Form.Item name="body" label="Body" initialValue={template?.body}>
            <TextArea rows={14} />
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
};

export default TemplateForm;
