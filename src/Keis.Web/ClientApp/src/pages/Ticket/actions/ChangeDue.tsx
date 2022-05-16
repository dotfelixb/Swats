import { Button, DatePicker, Form, Modal, Timeline } from "antd";
import moment from "moment";
import React, { FC } from "react";

interface IChangeDue {
  showModal: boolean;
  onHideModal: (value: React.SetStateAction<boolean>) => void;
  onSubmit: (values: any) => void;
}

const ChangeDue: FC<IChangeDue> = ({ showModal, onHideModal, onSubmit }) => {
  return (
    <Modal
      visible={showModal}
      title="Change ticket assign Team"
      onCancel={() => onHideModal(false)}
      destroyOnClose={true}
      footer={null}
    >
      <Form layout="vertical" onFinish={onSubmit}>
        <Timeline>
          <Timeline.Item>
            <div className="font-bold mb-2">When should work end</div>
            <Form.Item name="dueAt" label="Due Date">
              <DatePicker className="w-full" disabledDate={(current)=>{
                return moment().add(-1, 'days') >= current;
              }} />
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

export default ChangeDue;
