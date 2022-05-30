import { CommentOutlined, FormOutlined, TeamOutlined } from "@ant-design/icons";
import { Alert, Button, Form, Input, Select, Timeline } from "antd";
import React, { FC } from "react";
import { IFetchAgent, IFetchDepartment, IFetchTeam } from "../../interfaces";

const { TextArea } = Input;

interface ITeamForm {
  hasFormErrors: boolean;
  formErrors: string[];
  team?: IFetchTeam;
  departmentList: IFetchDepartment[];
  agentList: IFetchAgent[];
  onFinish: (values: any) => void;
}

const TeamForm: FC<ITeamForm> = ({
  hasFormErrors,
  formErrors,
  team,
  departmentList,
  agentList,
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
          <div className="font-bold mb-2">Team name</div>
          <Form.Item name="name" label="Name" initialValue={team?.name}>
            <Input />
          </Form.Item>
        </Timeline.Item>
        <Timeline.Item dot={<TeamOutlined style={{ fontSize: "16px" }} />}>
          <div className="font-bold mb-2">Team lead, department and status</div>
          <Form.Item name="manager" label="Team Lead" initialValue={team?.manager}>
            <Select>
              {agentList?.map((d) => (
                <Select.Option key={d.id} value={d.id}>
                  {d.name}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item name="department" label="Team Department" initialValue={team?.department}>
            <Select>
              {departmentList?.map((d) => (
                <Select.Option key={d.id} value={d.id}>
                  {d.name}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item name="status" label="Status" initialValue={team?.status}>
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
          <Form.Item name="response" label="Default Response" initialValue={team?.response}>
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

export default TeamForm;
