import { createContext, FC, useContext } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { IAppContext, IViewProps } from "../interfaces";
import { useAuth } from "./AuthContext";

export const AppContext = createContext<IAppContext>(null!);

export const useApp = () => useContext(AppContext);

export const AppProvider: FC<IViewProps> = ({ children }) => {
  const { user } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const dateFormats = {
    longDateFormat: "MMM DD, YYYY h:mm a",
    longDateFormatWithAt: "MMM DD, YYYY @ h:mm a",
    shortDateFormat: "MMM DD, YYYY",
  };
  const editorFormats = [
    "header",
    "font",
    "size",
    "bold",
    "italic",
    "underline",
    "strike",
    "blockquote",
    "list",
    "bullet",
    "indent",
    "link",
    "image",
    "video",
  ];
  const editorModels = {
    toolbar: [
      [{ header: "1" }, { header: "2" }, { font: [] }],
      [{ size: [] }],
      ["bold", "italic", "underline", "strike", "blockquote"],
      [
        { list: "ordered" },
        { list: "bullet" },
        { indent: "-1" },
        { indent: "+1" },
      ],
      ["link", "image", "video"],
      ["clean"],
    ],
    clipboard: {
      // toggle to add extra line breaks when pasting HTML:
      matchVisual: false,
    },
  };

  const post = async (endPoint: string, body: FormData): Promise<any> => {
    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await fetch(endPoint, {
      method: "POST",
      headers,
      body,
    });

    return f.status === 401
      ? navigate("/login", { replace: true, state: { from: location } })
      : f;
  };

  const get = async (endPoint: string): Promise<any> => {
    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await fetch(endPoint, {
      method: "GET",
      headers,
    });

    return f.status === 401
      ? navigate("/login", { replace: true, state: { from: location } })
      : f;
  };

  const patch = async (endPoint: string, body: FormData): Promise<any> => {
    const headers = new Headers();
    headers.append("Authorization", `Bearer ${user?.token ?? ""}`);

    const f = await fetch(endPoint, {
      method: "PATCH",
      headers,
      body,
    });

    return f.status === 401
      ? navigate("/login", { replace: true, state: { from: location } })
      : f;
  };

  return (
    <AppContext.Provider value={{ post, get, patch, dateFormats, editorFormats, editorModels }}>
      {children}
    </AppContext.Provider>
  );
};
