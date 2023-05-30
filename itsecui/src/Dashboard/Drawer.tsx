import * as React from "react";
import Box from "@mui/material/Box";
import Drawer from "@mui/material/Drawer";
import Button from "@mui/material/Button";
import List from "@mui/material/List";
import Divider from "@mui/material/Divider";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import DehazeIcon from "@mui/icons-material/Dehaze";
import { User } from "../AuthenticatedUser";
import SettingsPage from "../SettingsPage";
import { useLocation, useNavigate } from "react-router-dom";

type Anchor = "top" | "left" | "bottom" | "right";

interface IProps {
  user?: User;
}

export default function SideDrawer(props: IProps) {
  const navigation = useNavigate();
  const location = useLocation();
  const [state, setState] = React.useState({
    top: false,
    left: false,
    bottom: false,
    right: false,
    props: props,
  });

  React.useEffect(() => {
    if (state.props.user === null) {
      console.log(location.state);
      setState({ ...state, props: (props.user = location.state) });
    }
  });

  const elements =
    props.user?.role === "admin" ? ["Dashboard", "Admin"] : ["Dashboard"];

  const toggleDrawer =
    (anchor: Anchor, open: boolean) =>
    (event: React.KeyboardEvent | React.MouseEvent) => {
      if (
        event.type === "keydown" &&
        ((event as React.KeyboardEvent).key === "Tab" ||
          (event as React.KeyboardEvent).key === "Shift")
      ) {
        return;
      }

      setState({ ...state, [anchor]: open });
    };

  const list = (anchor: Anchor) => (
    <Box
      sx={{ width: anchor === "top" || anchor === "bottom" ? "auto" : 250 }}
      role="presentation"
      onClick={toggleDrawer(anchor, false)}
      onKeyDown={toggleDrawer(anchor, false)}
    >
      <List>
        {elements.map((text, index) => (
          <ListItem key={text} disablePadding>
            <ListItemButton>
              <ListItemText
                primary={text}
                onClick={() => {
                  console.log(text);
                  switch (text) {
                    case "Admin":
                      navigation("/admin", { state: state.props.user });
                      break;
                    case "Dashboard":
                      navigation("/", { state: state.props.user });
                  }
                }}
              />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
      <Divider />
      <List>
        {["About"].map((text, index) => (
          <ListItem key={text} disablePadding>
            <ListItemButton>
              <ListItemText primary={text} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </Box>
  );

  return (
    <div>
      {(["left"] as const).map((anchor) => (
        <React.Fragment key={anchor}>
          <Button onClick={toggleDrawer(anchor, true)}>
            <DehazeIcon></DehazeIcon>
          </Button>
          <Drawer
            anchor={anchor}
            open={state[anchor]}
            onClose={toggleDrawer(anchor, false)}
          >
            {list(anchor)}
          </Drawer>
        </React.Fragment>
      ))}
    </div>
  );
}
