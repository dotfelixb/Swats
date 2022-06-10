import {
  Breadcrumb,
  Timeline,
  Button,
  Menu,
  MenuProps,
  Dropdown,
  Modal,
  message,
} from "antd";
import dayjs from "dayjs";
import React, { FC, useCallback, useEffect, useState } from "react";
import DOMPurify from "dompurify";
import ReactQuill from "react-quill";
import { Link, useParams } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import {
  IFetchAgent,
  IFetchDepartment,
  IFetchTeam,
  IFetchTicket,
  IFetchTopic,
  IFetchType,
  IListResult,
  ISingleResult,
} from "../../interfaces";
import {
  ColumnHeightOutlined,
  CommentOutlined,
  DoubleRightOutlined,
  FormOutlined,
  MoreOutlined,
  SwapOutlined,
} from "@ant-design/icons";
import {
  ChangeAgent,
  ChangeDepartment,
  ChangeTeam,
  ChangeTicketType,
  ChangeTopic,
} from "./actions";
import { ReplyIcon } from "../../components/Icons";
import ChangeDue from "./actions/ChangeDue";

interface IViewTicket {}

const statusItems = [
  {
    key: 1,
    label: "New",
  },
  {
    key: 2,
    label: "Open",
  },
  {
    key: 3,
    label: "Approved",
  },
  {
    key: 4,
    label: "Assigned",
  },
  {
    key: 5,
    label: "Pending",
  },
  {
    key: 6,
    label: "Review",
  },
  {
    key: 7,
    label: "Close",
  },
  {
    key: 8,
    label: "Deleted",
  },
];

const priorityItems = [
  {
    key: 1,
    label: "Low",
  },
  {
    key: 2,
    label: "Normal",
  },
  {
    key: 3,
    label: "High",
  },
  {
    key: 4,
    label: "Important!",
  },
];

const ViewTicket: FC<IViewTicket> = () => {
  const { user } = useAuth();
  const { get, post, patch, dateFormats, editorFormats, editorModels } = useApp();
  const { id } = useParams();
  const [note, setNote] = useState("");
  const [showComment, setShowComment] = useState(false);
  const [ticket, setTicket] = useState<IFetchTicket>();
  const [showChangeStatusModal, setChangeStatusModal] = useState(false);
  const [showChangePriorityModal, setChangePriorityModal] = useState(false);
  const [showAssignedToModal, setAssignedToModal] = useState(false);
  const [showDepartmentModal, setDepartmentModal] = useState(false);
  const [showTeamModal, setTeamModal] = useState(false);
  const [showDueModal, setDueModal] = useState(false);
  const [showTicketTypeModal, setTicketTypeModal] = useState(false);
  const [showHelpTopicModal, setHelpTopicModal] = useState(false);
  const [newStatus, setNewStatus] = useState<string>();
  const [newPriority, setNewPriority] = useState<string>();
  const [agentList, setAgentList] = useState<IFetchAgent[]>([]);
  const [departmentList, setDepartmentList] = useState<IFetchDepartment[]>([]);
  const [teamList, setTeamList] = useState<IFetchTeam[]>([]);
  const [typeList, setTypeList] = useState<IFetchType[]>([]);
  const [topicList, setTopicList] = useState<IFetchTopic[]>([]);

  const load = useCallback(async () => {
    const g: Response = await get(`methods/ticket.get?id=${id}`);
    const d: ISingleResult<IFetchTicket> = await g.json();

    if (g.status === 200 && d.ok) {
      setTicket(d.data);
    } else {
      // TODO: display error to user
    }
  }, [get, id]);

  const loadAgent = useCallback(async () => {
    const g: Response = await get(`methods/agent.list`);
    const d: IListResult<IFetchAgent[]> = await g.json();

    if (g.status === 200 && d.ok) {
      setAgentList(d.data);
    } else {
      // TODO: display error to user
    }
  }, [get]);

  const loadDepartment = useCallback(async () => {
    const g: Response = await get(`methods/department.list`);
    const d: IListResult<IFetchDepartment[]> = await g.json();

    if (g.status === 200 && d.ok) {
      setDepartmentList(d.data);
    } else {
      // TODO: display error to user
    }
  }, [get]);

  const loadTeam = useCallback(async () => {
    const g: Response = await get(`methods/team.list`);
    const d: IListResult<IFetchTeam[]> = await g.json();

    if (g.status === 200 && d.ok) {
      setTeamList(d.data);
    } else {
      // TODO: display error to user
    }
  }, [get]);

  const loadType = useCallback(async () => {
    const g: Response = await get(`methods/tickettype.list`);
    const d: IListResult<IFetchType[]> = await g.json();

    if (g.status === 200 && d.ok) {
      setTypeList(d.data);
    } else {
      // TODO: display error to user
    }
  }, [get]);

  const loadTopic = useCallback(async () => {
    const g: Response = await get(`methods/helptopic.list`);
    const d: IListResult<IFetchTopic[]> = await g.json();

    if (g.status === 200 && d.ok) {
      setTopicList(d.data);
    } else {
      // TODO: display error to user
    }
  }, [get]);

  useEffect(() => {
    if (user != null && user.token && id) {
      load();
    }
  }, [user, id, get, load]);

  useEffect(() => {
    if (showAssignedToModal) {
      loadAgent();
    }

    if (showDepartmentModal) {
      loadDepartment();
    }

    if (showTeamModal) {
      loadTeam();
    }

    if (showTicketTypeModal) {
      loadType();
    }

    if (showHelpTopicModal) {
      loadTopic();
    }
  }, [
    showAssignedToModal,
    showDepartmentModal,
    showTeamModal,
    showTicketTypeModal,
    showHelpTopicModal,

    loadAgent,
    loadDepartment,
    loadTeam,
    loadType,
    loadTopic,
  ]);

  const onHandleStatusChange: MenuProps["onClick"] = (e) => {
    setNewStatus(e.key);
    setChangeStatusModal(true);
  };

  const onHandlePriorityChange: MenuProps["onClick"] = (e) => {
    setNewPriority(e.key);
    setChangePriorityModal(true);
  };

  const onPatch = useCallback(
    async (
      body: FormData,
      uri: string,
      onSuccess: (id: string, name: string) => void,
      onHideModal: (value: boolean) => void
    ) => {
      const f: Response = await patch(uri, body);
      const result: ISingleResult<{ name: string; id: string }> =
        await f.json();

      if (f.status === 200 && result.ok) {
        onSuccess(result.data?.id ?? result.data, result.data?.name ?? "");
        onHideModal(false);
      }
    },
    [patch]
  );

  const onSubmitChangeStatus = useCallback(async () => {
    const body = new FormData();
    body.append("status", newStatus ?? "");
    body.append("id", ticket?.id ?? "");

    const onSuccess = (id: string, name: string) => {
      if (ticket !== undefined) {
        setTicket({ ...ticket, status: id });
      }

      message.success(`Ticket status successfully set to ${id}`);
    };

    onPatch(body, "methods/ticket.status", onSuccess, setChangeStatusModal);
  }, [onPatch, ticket, newStatus]);

  const onSubmitChangePriority = useCallback(async () => {
    const body = new FormData();
    body.append("priority", newPriority ?? "");
    body.append("id", ticket?.id ?? "");

    const onSuccess = (id: string, name: string) => {
      if (ticket !== undefined) {
        setTicket({ ...ticket, priority: id });
      }

      message.success(`Ticket priority successfully set to ${id}`);
    };

    onPatch(body, "methods/ticket.priority", onSuccess, setChangePriorityModal);
  }, [onPatch, ticket, newPriority]);

  const onSubmitAssignedTo = useCallback(
    async (values: any) => {
      const body = new FormData();
      body.append("assignedto", values.assignedto ?? "");
      body.append("id", ticket?.id ?? "");

      const onSuccess = (id: string, name: string) => {
        if (ticket !== undefined) {
          setTicket({
            ...ticket,
            assignedTo: id,
            assignedToName: name,
          });
        }

        message.success(`Ticket successfully assigned to ${name}`);
      };

      onPatch(body, "methods/ticket.assign", onSuccess, setAssignedToModal);
    },
    [ticket, onPatch]
  );

  const onSubmitChangeDepartment = useCallback(
    async (values: any) => {
      const body = new FormData();
      body.append("department", values.department ?? "");
      body.append("id", ticket?.id ?? "");

      const onSuccess = (id: string, name: string) => {
        if (ticket !== undefined) {
          setTicket({ ...ticket, department: id, departmentName: name });
        }

        message.success(`Ticket department successfully changed to ${name}`);
      };

      onPatch(body, "methods/ticket.department", onSuccess, setDepartmentModal);
    },
    [onPatch, ticket]
  );

  const onSubmitChangeTeam = useCallback(
    async (values: any) => {
      const body = new FormData();
      body.append("team", values.team ?? "");
      body.append("id", ticket?.id ?? "");

      const onSuccess = (id: string, name: string) => {
        if (ticket !== undefined) {
          setTicket({ ...ticket, team: id, teamName: name });
        }

        message.success(`Ticket team successfully changed to ${name}`);
      };

      onPatch(body, "methods/ticket.team", onSuccess, setTeamModal);
    },
    [onPatch, ticket]
  );

  const onSubmitChangeDueDate = useCallback(
    async (values: any) => {
      const body = new FormData();
      body.append("dueAt", dayjs(values.dueAt ?? "").format() ?? "");
      body.append("id", ticket?.id ?? "");

      const onSuccess = (id: string, name: string) => {
        if (ticket !== undefined) {
          setTicket({ ...ticket, dueAt: id });
        }

        const due = dayjs(values.dueAt).format(dateFormats.shortDateFormat);
        message.success(`Ticket Due Date successfully changed to ${due}`);
      };

      onPatch(body, "methods/ticket.duedate", onSuccess, setDueModal);
    },
    [dateFormats.shortDateFormat, onPatch, ticket]
  );

  const onSubmitChangeTicketType = useCallback(
    async (values: any) => {
      const body = new FormData();
      body.append("ticketType", values.ticketType ?? "");
      body.append("id", ticket?.id ?? "");

      const onSuccess = (id: string, name: string) => {
        if (ticket !== undefined) {
          setTicket({ ...ticket, ticketType: id, ticketTypeName: name });
        }

        message.success(`Ticket Type successfully changed to ${name}`);
      };

      onPatch(body, "methods/ticket.tickettype", onSuccess, setTicketTypeModal);
    },
    [onPatch, ticket]
  );

  const onSubmitChangeHelpTopic = useCallback(
    async (values: any) => {
      const body = new FormData();
      body.append("helpTopic", values.helpTopic ?? "");
      body.append("id", ticket?.id ?? "");

      const onSuccess = (id: string, name: string) => {
        if (ticket !== undefined) {
          setTicket({ ...ticket, helpTopic: id, helpTopicName: name });
        }

        message.success(`Ticket Help Topic successfully changed to ${name}`);
      };

      onPatch(body, "methods/ticket.helptopic", onSuccess, setHelpTopicModal);
    },
    [onPatch, ticket]
  );

  const onHandleMoreAction: MenuProps["onClick"] = (e) => {
    switch (parseInt(e.key)) {
      case 104: {
        setDueModal(true);
        break;
      }
      case 105: {
        setDepartmentModal(true);
        break;
      }
      case 106: {
        setTeamModal(true);
        break;
      }
      case 108: {
        setHelpTopicModal(true);
        break;
      }
      case 109: {
        setTicketTypeModal(true);
        break;
      }
      default:
        break;
    }
  };

  const onHandleComment = useCallback(async () => {
    const body = new FormData();
    body.append("ticketId", ticket?.id ?? "");
    body.append("body", note ?? "");

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await post("methods/ticket.postcomment",  body );

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      setNote("");
      setShowComment(false);

      // call load ticket
      await load();
    }
  }, [user, ticket, load, note]);

  const changeStatusMenu = (
    <Menu onClick={onHandleStatusChange} items={statusItems} />
  );

  const changePriorityMenu = (
    <Menu onClick={onHandlePriorityChange} items={priorityItems} />
  );

  const moreActionMenu = (
    <Menu
      onClick={onHandleMoreAction}
      items={[
        {
          key: 101,
          label: "Merge Tickets",
        },
        {
          key: 102,
          label: "Duplicate Ticket",
        },
        {
          key: 103,
          label: "Request Feedback",
        },
        {
          key: 104,
          label: "Change Due Date",
        },
        {
          key: "105",
          label: "Change Department",
        },
        {
          key: 106,
          label: "Change Team",
        },
        {
          key: 108,
          label: "Change Help Topic",
        },
        {
          key: 109,
          label: "Change Ticket Type",
        },
      ]}
    />
  );

  const Buttons: FC = () => (
    <div className="flex flex-row space-x-2">
      <div>
        <Dropdown overlay={changeStatusMenu}>
          <Button
            type="default"
            size="small"
            style={{ display: "flex", alignItems: "center" }}
            icon={<SwapOutlined />}
          >
            Change Status
          </Button>
        </Dropdown>
      </div>
      <div>
        <Dropdown overlay={changePriorityMenu}>
          <Button
            type="default"
            size="small"
            style={{ display: "flex", alignItems: "center" }}
            icon={<ColumnHeightOutlined />}
          >
            Change Priority
          </Button>
        </Dropdown>
      </div>
      <div>
        <Button
          type="default"
          size="small"
          style={{ display: "flex", alignItems: "center" }}
          icon={<DoubleRightOutlined />}
          onClick={() => setAssignedToModal(true)}
        >
          Assign To
        </Button>
      </div>
      <div>
        <Button
          type="default"
          size="small"
          style={{ display: "flex", alignItems: "center" }}
          icon={<FormOutlined />}
          onClick={() => setShowComment(true)}
        >
          Add Comment
        </Button>
      </div>
      <div>
        <Dropdown overlay={moreActionMenu}>
          <Button type="default" size="small">
            <div
              className="space-x-2"
              style={{ display: "flex", alignItems: "center" }}
            >
              <span>Actions</span>
              <MoreOutlined />
            </div>
          </Button>
        </Dropdown>
      </div>
    </div>
  );

  const Breadcrumbs: FC = () => (
    <Breadcrumb separator="/">
      <Breadcrumb.Item>
        <Link to="/">Home</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/ticket">Tickets</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>{ticket?.code ?? "View Ticket"}</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title={ticket?.subject ?? "View Ticket"}
      breadcrumbs={<Breadcrumbs />}
      buttons={<Buttons />}
    >
      <div className="w-full flex flex-row ">
        <div style={{ width: "220px" }} className="">
          <div className="pr-2">
            <div className="bg-gray-200 rounded-sm w-28 h-28"></div>
          </div>
          <ul className="edit-sidebar py-5">
            <li>
              <div>Assigned To</div>
              <div>{ticket?.assignedToName ?? "N/A"}</div>
            </li>
            <li>
              <div>Department</div>
              <div>{ticket?.departmentName ?? "N/A"}</div>
            </li>
            <li>
              <div>Team</div>
              <div>{ticket?.teamName ?? "N/A"}</div>
            </li>
            <li>
              <div>Applied SLA</div>
              <div>{ticket?.slaName ?? "N/A"}</div>
            </li>
            <li>
              <div>Priority</div>
              <div>{ticket?.priority ?? "N/A"}</div>
            </li>
            <li>
              <div>Status</div>
              <div>{ticket?.status ?? "N/A"}</div>
            </li>
            <li>
              <div>Source</div>
              <div>{ticket?.source ?? "N/A"}</div>
            </li>
            <li>
              <div>Due Date</div>
              <div>
                {ticket?.dueAt
                  ? dayjs(ticket?.dueAt).format(dateFormats.shortDateFormat)
                  : "N/A"}
              </div>
            </li>
            <li>
              <div>Default Ticket Type</div>
              <div>{ticket?.ticketTypeName ?? "N/A"}</div>
            </li>
            <li>
              <div>Help Topic</div>
              <div>{ticket?.helpTopicName ?? "N/A"}</div>
            </li>
            <li>
              <div>Created By</div>
              <div>{ticket?.createdByName ?? "N/A"}</div>
            </li>
            <li>
              <div>Created At</div>
              <div>
                {dayjs(ticket?.createdAt).format(dateFormats.shortDateFormat)}
              </div>
            </li>
            <li>
              <div>Updated By</div>
              <div>{ticket?.updatedByName ?? "N/A"}</div>
            </li>
            <li>
              <div>Updated At</div>
              <div>
                {dayjs(ticket?.updatedAt).format(dateFormats.shortDateFormat)}
              </div>
            </li>
          </ul>
        </div>

        <div className="w-full bg-white border border-gray-200 rounded-sm px-10 py-10">
          <Timeline>
            {showComment && (
              <>
                <Timeline.Item
                  dot={<CommentOutlined style={{ fontSize: "16px" }} />}
                >
                  <div className="font-bold mb-2">Add your comment</div>

                  <div style={{height: "200px"}}>
                    <ReactQuill
                      theme="snow"
                      value={note}
                      onChange={setNote}
                      formats={editorFormats}
                      modules={editorModels} 
                      style={{ height: "65%" }}
                    />
                  </div>
                </Timeline.Item>
                <Timeline.Item>
                  <div className="flex flex-row space-x-3">
                    <Button
                      type="primary"
                      size="small"
                      onClick={onHandleComment}
                    >
                      Update
                    </Button>
                    <Button
                      type="default"
                      size="small"
                      onClick={() => setShowComment(false)}
                    >
                      Cancel
                    </Button>
                  </div>
                  <div className="mb-5"></div>
                </Timeline.Item>
              </>
            )}
            {ticket?.ticketComments?.map((c) => {
              const __html = DOMPurify.sanitize(c.body);

              return (
                <Timeline.Item
                  key={c.id}
                  dot={<CommentOutlined style={{ fontSize: "16px" }} />}
                >
                  <div className="flex w-full">
                    {/* commenter name */}
                    <div className="mb-2 flex w-1/2 flex-row">
                      <div>{c.fromName}</div>
                      <div className="px-1 flex flex-row text-xs items-center">
                        <div>{" - "}</div>
                        <div>
                          {dayjs(c.createdAt).format(
                            dateFormats.longDateFormat
                          )}
                        </div>
                      </div>
                    </div>
                    {/* reply button */}
                    <div className="flex w-1/2 justify-end">
                      <Button
                        type="default"
                        size="small"
                        style={{ display: "flex", alignItems: "center" }}
                        icon={<ReplyIcon />}
                        onClick={() => setShowComment(true)}
                      >
                        Reply
                      </Button>
                    </div>
                  </div>
                  <div
                    className="px-4 py-3 border rounded"
                    dangerouslySetInnerHTML={{ __html }}
                  ></div>
                </Timeline.Item>
              );
            })}
          </Timeline>
        </div>
      </div>

      {/* change status modal */}
      <Modal
        visible={showChangeStatusModal}
        title="Change Ticket Status"
        okText="Update"
        onOk={onSubmitChangeStatus}
        onCancel={() => setChangeStatusModal(false)}
      >
        <div className="text-base">
          You are about to change ticket Status to
          <span className="font-bold">
            {" '"}
            {
              statusItems.find((f) => f.key === parseInt(newStatus ?? "0"))
                ?.label
            }
            {"' "}
          </span>
        </div>
      </Modal>

      <Modal
        visible={showChangePriorityModal}
        title="Change Ticket Priority"
        okText="Update"
        onOk={onSubmitChangePriority}
        onCancel={() => setChangePriorityModal(false)}
      >
        <div className="text-base">
          You are about to change ticket Priority to
          <span className="font-bold">
            {" '"}
            {
              priorityItems.find((f) => f.key === parseInt(newPriority ?? "0"))
                ?.label
            }
            {"' "}
          </span>
        </div>
      </Modal>

      <ChangeAgent
        showModal={showAssignedToModal}
        agentList={agentList}
        onHideModal={setAssignedToModal}
        onSubmit={onSubmitAssignedTo}
      />

      <ChangeDepartment
        showModal={showDepartmentModal}
        departmentList={departmentList}
        onHideModal={setDepartmentModal}
        onSubmit={onSubmitChangeDepartment}
      />

      <ChangeTeam
        showModal={showTeamModal}
        teamList={teamList}
        onHideModal={setTeamModal}
        onSubmit={onSubmitChangeTeam}
      />

      <ChangeDue
        showModal={showDueModal}
        onHideModal={setDueModal}
        onSubmit={onSubmitChangeDueDate}
      />

      <ChangeTicketType
        showModal={showTicketTypeModal}
        typeList={typeList}
        onHideModal={setTicketTypeModal}
        onSubmit={onSubmitChangeTicketType}
      />

      <ChangeTopic
        showModal={showHelpTopicModal}
        topicList={topicList}
        onHideModal={setHelpTopicModal}
        onSubmit={onSubmitChangeHelpTopic}
      />
    </PageView>
  );
};

export default ViewTicket;
