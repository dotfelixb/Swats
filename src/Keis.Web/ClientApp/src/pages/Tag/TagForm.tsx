import {
  ClockCircleOutlined,
  CommentOutlined,
  FormOutlined,
} from "@ant-design/icons";
import { Alert, Button, Form, Input, Select, Timeline } from "antd";
import React, { FC } from "react";
import { IFetchTag } from "../../interfaces";

const { TextArea } = Input;

interface ITagForm {
  hasFormErrors: boolean;
  formErrors: string[];
  tag?: IFetchTag
  onFinish: (values: any) => void;
}

const TagForm: FC<ITagForm> = ({ hasFormErrors, formErrors, tag, onFinish }) => (
  <Form layout="vertical" onFinish={onFinish}>
    {hasFormErrors &&
      formErrors?.map((e) => (
        <div key={e} className="py-2">
          <Alert message={e} type="error" className="py-2" />
        </div>
      ))}
    <Timeline>
      <Timeline.Item dot={<FormOutlined style={{ fontSize: "16px" }} />}>
        <div className="font-bold mb-2">Tag name</div>
        <Form.Item name="name" label="Name" initialValue={tag?.name}>
          <Input />
        </Form.Item>
      </Timeline.Item>
      <Timeline.Item dot={<ClockCircleOutlined style={{ fontSize: "16px" }} />}>
        <div className="font-bold mb-2">Visibility and status</div>
        <Form.Item name="visibility" label="Visibility" initialValue={tag?.visibility}>
          <Select>
            <Select.Option value="1">Public</Select.Option>
            <Select.Option value="2">Private</Select.Option>
          </Select>
        </Form.Item>
        <Form.Item name="status" label="Status" initialValue={tag?.status}>
          <Select>
            <Select.Option value="1">Active</Select.Option>
            <Select.Option value="2">Inactive</Select.Option>
          </Select>
        </Form.Item>
      </Timeline.Item>
      <Timeline.Item dot={<CommentOutlined style={{ fontSize: "16px" }} />}>
        <div className="font-bold mb-2">
          Addition note for this tag (optional)
        </div>
        <Form.Item name="note" label="Note" initialValue={tag?.note} >
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

export default TagForm;
