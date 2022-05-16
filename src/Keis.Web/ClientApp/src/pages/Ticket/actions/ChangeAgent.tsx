import { UserOutlined } from "@ant-design/icons";
import { Button, Form, Modal, Select, Timeline } from "antd";
import React, { FC } from "react";
import { IFetchAgent } from "../../../interfaces";

interface IChangeAgent {
  showModal: boolean;
  agentList: IFetchAgent[];
  onHideModal: (value: React.SetStateAction<boolean>) => void;
  onSubmit: (values: any) => void;
}

const ChangeAgent: FC<IChangeAgent> = ({
  showModal,
  agentList,
  onHideModal,
  onSubmit,
}) => {
  return (
    <Modal
      visible={showModal}
      title="Assign ticket to Agent"
      onCancel={() => onHideModal(false)}
      destroyOnClose={true}
      footer={null}
    >
      <Form layout="vertical" onFinish={onSubmit}>
        <Timeline>
          <Timeline.Item dot={<UserOutlined style={{ fontSize: "16px" }} />}>
            <div className="font-bold mb-2">Who should work on it</div>
            <Form.Item name="assignedto" label="Assign to">
              <Select showSearch>
                {agentList?.map((a) => (
                  <Select.Option key={a.id} value={a.id}>
                    {a.name}
                  </Select.Option>
                ))}
              </Select>
            </Form.Item>
          </Timeline.Item>
          <Timeline.Item>
            <Form.Item>
              <Button type="primary" htmlType="submit">
                Update
              </Button>
            </Form.Item>
          </Timeline.Item>
        </Timeline>
      </Form>
    </Modal>
  );
};

export default ChangeAgent;
