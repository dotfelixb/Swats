import { CommentOutlined, EyeOutlined, FormOutlined } from "@ant-design/icons";
import { Alert, Button, Form, Input, Select, Timeline } from "antd";
import React, { FC } from "react";
import { IFetchAgent, IFetchBusinessHour, IFetchDepartment } from "../../interfaces";

const { TextArea } = Input;

interface IDepartmentForm {
  hasFormErrors: boolean;
  formErrors: string[];
  department?: IFetchDepartment;
  agentList: IFetchAgent[];
  hourList:IFetchBusinessHour[];
  onFinish: (values: any) => void;
}

const DepartmentForm: FC<IDepartmentForm> = ({
  hasFormErrors,
  formErrors,
  department,
  agentList,
  hourList,
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
          <div className="font-bold mb-2">Department name and type</div>
          <Form.Item name="name" label="Name" initialValue={department?.name}>
            <Input />
          </Form.Item>
          <Form.Item name="type" label="Type" initialValue={department?.type}>
            <Select>
              <Select.Option value="1">Public</Select.Option>
              <Select.Option value="2">Private</Select.Option>
            </Select>
          </Form.Item>
        </Timeline.Item>
        <Timeline.Item dot={<EyeOutlined style={{ fontSize: "16px" }} />}>
          <div className="font-bold mb-2">
            Department lead, working hour and status
          </div>
          <Form.Item name="manager" label="Manager" initialValue={department?.manager}>
            <Select>
              {agentList?.map((d) => (
                <Select.Option key={d.id} value={d.id}>
                  {d.name}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item name="businessHour" label="Business Hour" initialValue={department?.businessHour}>
            <Select>
              {hourList?.map((d) => (
                <Select.Option key={d.id} value={d.id}>
                  {d.name}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item
            name="outgoingEmail"
            label="Outgoing Email" initialValue={department?.outgoingEmail}
          >
            <Input />
          </Form.Item>
          <Form.Item name="status" label="Status" initialValue={department?.status}>
            <Select>
              <Select.Option value="1">Active</Select.Option>
              <Select.Option value="2">Inactive</Select.Option>
            </Select>
          </Form.Item>
        </Timeline.Item>
        <Timeline.Item dot={<CommentOutlined style={{ fontSize: "16px" }} />}>
          <div className="font-bold mb-2">
            The default response to send in email (optional)
          </div>
          <Form.Item
            name="response"
            label="Default Response" initialValue={department?.response}
          >
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
};

export default DepartmentForm;
