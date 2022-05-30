import { FC } from "react";
import { Route, Routes } from "react-router-dom";
import "./site.css";
import "./custom.less";
import {
  Dashboard,
  ListAgents,
  ListDepartments,
  ListHours,
  ListSla,
  ListTags,
  ListTeams,
  ListTickets,
  ListTopics,
  ListTypes,
  ListWorkflow,
  Login,
  NewAgent,
  NewDepartment,
  NewHour,
  NewSla,
  NewTag,
  NewTeam,
  NewTicket,
  NewTopic,
  NewType,
  NewWorkflow,
  NoMatch,
  Settings,
  ViewAgent,
  ViewDepartment,
  ViewHour,
  ViewSla,
  ViewTag,
  ViewTeam,
  ViewTicket,
  ViewTopic,
  ViewType,
  ViewWorkflow,
} from "./pages";
import { Loader, MainLayout } from "./components";
import { AppProvider, AuthProvider, useAuth } from "./context";

const Ready: FC = () => {
  const { browserLoaded } = useAuth();

  if (!browserLoaded) {
    return (
      <Loader />
    );
  }

  return (
    <Routes>
      <Route path="/" element={<MainLayout />}>
        <Route index element={<Dashboard />} />
        <Route path="ticket">
          <Route index element={<ListTickets />} />
          <Route path="new" element={<NewTicket />} />
          <Route path=":id" element={<ViewTicket />} />
        </Route>
        <Route path="agent">
          <Route index element={<ListAgents />} />
        </Route>
        <Route path="admin">
          <Route index element={<Settings />} />
          <Route path="agent">
            <Route index element={<ListAgents />} />
            <Route path="new" element={<NewAgent />} />
            <Route path=":id" element={<ViewAgent />} />
          </Route>
          <Route path="department">
            <Route index element={<ListDepartments />} />
            <Route path="new" element={<NewDepartment />} />
            <Route path=":id" element={<ViewDepartment />} />
          </Route>
          <Route path="team">
            <Route index element={<ListTeams />} />
            <Route path="new" element={<NewTeam />} />
            <Route path=":id" element={<ViewTeam />} />
          </Route>
          <Route path="helptopic">
            <Route index element={<ListTopics />} />
            <Route path="new" element={<NewTopic />} />
            <Route path=":id" element={<ViewTopic />} />
          </Route>
          <Route path="tickettype">
            <Route index element={<ListTypes />} />
            <Route path="new" element={<NewType />} />
            <Route path=":id" element={<ViewType />} />
          </Route>
          <Route path="businesshour">
            <Route index element={<ListHours />} />
            <Route path="new" element={<NewHour />} />
            <Route path=":id" element={<ViewHour />} />
          </Route>
          <Route path="tag">
            <Route index element={<ListTags />} />
            <Route path="new" element={<NewTag />} />
            <Route path=":id" element={<ViewTag />} />
          </Route>
          <Route path="sla">
            <Route index element={<ListSla />} />
            <Route path="new" element={<NewSla />} />
            <Route path=":id" element={<ViewSla />} />
          </Route>
          <Route path="workflow">
            <Route index element={<ListWorkflow />} />
            <Route path="new" element={<NewWorkflow />} />
            <Route path=":id" element={<ViewWorkflow />} />
          </Route>
        </Route>
      </Route>
      <Route path="/login" element={<Login />} />
      <Route path="*" element={<NoMatch />} />
    </Routes>
  );
};

const App: FC = () => (
  <AuthProvider>
    <AppProvider>
      <Ready />
    </AppProvider>
  </AuthProvider>
);

export default App;
