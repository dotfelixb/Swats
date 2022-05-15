import { Button, Form, Modal, Select, Timeline } from "antd";
import React, { FC } from "react";
import { IFetchType } from "../../../interfaces";

interface IChangeTicketType {
  showModal: boolean;
  typeList: IFetchType[];
  onHideModal: (value: React.SetStateAction<boolean>) => void;
  onSubmit: (values: any) => void;
}

const ChangeTicketType: FC<IChangeTicketType> = ({
  showModal,
  typeList,
  onHideModal,
  onSubmit,
}) => {
  return (
    <Modal
      visible={showModal}
      title="Change assign Ticket Type"
      onCancel={() => onHideModal(false)}
      destroyOnClose={true}
      footer={null}
    >
      <Form layout="vertical" onFinish={onSubmit}>
        <Timeline>
          <Timeline.Item>
            <div className="font-bold mb-2">Who should work on it</div>
            <Form.Item name="ticketType" label="Ticket Type">
              <Select showSearch>
                {typeList?.map((a) => (
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

export default ChangeTicketType;
