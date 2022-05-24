import {
  CommentOutlined,
  CustomerServiceOutlined,
  PhoneOutlined,
  UserSwitchOutlined,
} from "@ant-design/icons";
import { Alert, Button, Form, Input, Select, Timeline } from "antd";
import React, { FC } from "react";
import { IFetchAgent, IFetchDepartment, IFetchTeam, IFetchType } from "../../interfaces";

const { TextArea } = Input;

interface INewForm {
  hasFormErrors: boolean;
  formErrors: string[];
  agent?: IFetchAgent;
  departmentList: IFetchDepartment[];
  teamList: IFetchTeam[];
  typeList: IFetchType[];
  onFinish: (values: any) => void;
}

const AgentForm: FC<INewForm> = ({
  hasFormErrors,
  formErrors,
  agent,
  departmentList,
  teamList,
  typeList,
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
        <Timeline.Item
          dot={<CustomerServiceOutlined style={{ fontSize: "16px" }} />}
        >
          <div className="font-bold mb-2">Agent info</div>
          <Form.Item name="firstname" label="First Name" initialValue={agent?.firstName}>
            <Input />
          </Form.Item>
          <Form.Item name="lastname" label="Last Name" initialValue={agent?.lastName}>
            <Input />
          </Form.Item>
          <Form.Item name="status" label="Status" initialValue={agent?.status}>
            <Select>
              <Select.Option value="1">Active</Select.Option>
              <Select.Option value="2">Inactive</Select.Option>
            </Select>
          </Form.Item>
        </Timeline.Item>
        <Timeline.Item dot={<PhoneOutlined style={{ fontSize: "16px" }} />}>
          <div className="font-bold mb-2">Agent contact</div>
          <Form.Item name="email" label="Email" initialValue={agent?.email}>
            <Input />
          </Form.Item>
          <Form.Item name="mobile" label="Mobile" initialValue={agent?.mobile}>
            <Input />
          </Form.Item>
          <Form.Item name="telephone" label="Telephone" initialValue={agent?.telephone}>
            <Input />
          </Form.Item>
          <Form.Item name="timezone" label="Timezone" initialValue={agent?.timezone}>
            <Select>
              <Select.Option value="">Not Available</Select.Option>
            </Select>
          </Form.Item>
        </Timeline.Item>
        <Timeline.Item
          dot={<UserSwitchOutlined style={{ fontSize: "16px" }} />}
        >
          <div className="font-bold mb-2">
            Agent department, team and service
          </div>
          <Form.Item name="department" label="Department" initialValue={agent?.department}>
            <Select>
              {departmentList?.map((d) => (
                <Select.Option key={d.id} value={d.id}>
                  {d.name}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item name="team" label="Team" initialValue={agent?.team}>
            <Select>
              {teamList?.map((d) => (
                <Select.Option key={d.id} value={d.id}>
                  {d.name}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item name="type" label="Default Ticket Type" initialValue={agent?.ticketType}>
            <Select>
              {typeList?.map((d) => (
                <Select.Option key={d.id} value={d.id}>
                  {d.name}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item name="mode" label="Mode" initialValue={agent?.mode}>
            <Select>
              <Select.Option value="1">Agent</Select.Option>
              <Select.Option value="2">User</Select.Option>
            </Select>
          </Form.Item>
        </Timeline.Item>
        <Timeline.Item dot={<CommentOutlined style={{ fontSize: "16px" }} />}>
          <div className="font-bold mb-2">
            Email signature for agent (optional)
          </div>
          <Form.Item name="note" label="Signature" initialValue={agent?.note}>
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

export default AgentForm;
