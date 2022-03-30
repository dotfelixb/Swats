import React, { FC } from "react";
import { Route, Routes } from "react-router-dom";
import "./site.css";
import "./custom.less";
import { Agent, Dashboard, NoMatch, Settings } from "./pages";
import { MainLayout } from "./components";
import { ListTicket, NewTicket, ViewTicket } from "./pages/Ticket";
import { AuthProvider } from "./context";
import Login from "./pages/Auth/Login";

const App: FC = () => (
  <AuthProvider>
    <Routes>
      <Route path="/" element={<MainLayout />}>
        <Route index element={<Dashboard />} />
        <Route path="ticket" >
          <Route index element={<ListTicket />} />
          <Route path="new" element={<NewTicket />} />
          <Route path=":ticketId" element={<ViewTicket />} />
        </Route>
        <Route path="/agent" element={<Agent />} />
        <Route path="/admin" element={<Settings />} />
      </Route>
      <Route path="/login" element={<Login />} />
      <Route path="*" element={<NoMatch />} />
    </Routes>
  </AuthProvider>
);

export default App;
