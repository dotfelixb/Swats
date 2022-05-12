import {
  Breadcrumb,
  Timeline,
  Button,
  Menu,
  MenuProps,
  Dropdown,
  Space,
} from "antd";
import dayjs from "dayjs";
import React, { FC, useEffect, useState } from "react";
import DOMPurify from "dompurify";
import ReactQuill from "react-quill";
import { Link, useParams } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchTicket, ISingleResult } from "../../interfaces";
import {
  CaretRightOutlined,
  CommentOutlined,
  DoubleRightOutlined,
  DownOutlined,
  FormOutlined,
  MoreOutlined,
  SwapOutlined,
} from "@ant-design/icons";

interface IViewTicket {}

const ReplyIcon = () => {
  return (
    <div className="pr-1">
      <svg
        viewBox="64 64 896 896"
        focusable="false"
        data-icon="form"
        width="1em"
        height="1em"
        fill="currentColor"
        aria-hidden="true"
      >
        <path
          d="M896 800c0 0-73.6-416-448-416l0-160L128 512l320 268.8 0-184.6C651.2 596.2 790 614 896 800z"
          p-id="1354"
        ></path>
      </svg>
    </div>
  );
};

const ViewTicket: FC<IViewTicket> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const { id } = useParams();
  const [note, setNote] = useState("");
  const [showComment, setShowComment] = useState(false);
  const [ticket, setTicket] = useState<IFetchTicket>();

  useEffect(() => {
    if (user != null && user.token && id) {
      load();
    }
  }, [user, id, get]);

  const load = async () => {
    const g: Response = await get(`methods/ticket.get?id=${id}`);
    const d: ISingleResult<IFetchTicket> = await g.json();

    if (g.status === 200 && d.ok) {
      setTicket(d.data);
    } else {
      // TODO: display error to user
    }
  };

  const onHandleStatusChange: MenuProps["onClick"] = (e) => {};
  const onHandleMoreAction: MenuProps["onClick"] = (e) => {};

  const onHandleComment = async () => {
    const body = new FormData();
    body.append("ticketId", ticket?.id ?? "");
    body.append("body", note ?? "");

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await fetch("methods/ticket.postcomment", {
      method: "POST",
      body,
      headers,
    });

    const result: ISingleResult<string> = await f.json();

    if (f.status === 201 && result.ok) {
      setNote("");
      setShowComment(false);

      // call load ticket
      await load();
    }
  };

  const changeStatusMenu = (
    <Menu
      onClick={onHandleStatusChange}
      items={[
        {
          key: 1,
          label: "New",
        },
        {
          key: 2,
          label: "Open",
        },
      ]}
    />
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
          key: 105,
          label: "Change Department",
        },
        {
          key: 106,
          label: "Change Team",
        },
        {
          key: 107,
          label: "Change Priority",
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
        <Button
          type="default"
          size="small"
          style={{ display: "flex", alignItems: "center" }}
          icon={<DoubleRightOutlined />}
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
              <div>Apply SLA</div>
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
              <div>Created By</div>
              <div>{ticket?.createdByName ?? "N/A"}</div>
            </li>
            <li>
              <div>Created At</div>
              <div>
                {dayjs(ticket?.createdAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
            <li>
              <div>Updated By</div>
              <div>{ticket?.updatedByName ?? "N/A"}</div>
            </li>
            <li>
              <div>Updated At</div>
              <div>
                {dayjs(ticket?.updatedAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
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

                  <ReactQuill
                    theme="snow"
                    value={note}
                    onChange={setNote}
                    style={{ height: "140px" }}
                  />
                  <div className="mb-9"></div>
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
    </PageView>
  );
};

export default ViewTicket;
