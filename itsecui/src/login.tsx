import * as React from "react";
import Button from "@mui/material/Button";
import CssBaseline from "@mui/material/CssBaseline";
import TextField from "@mui/material/TextField";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Container from "@mui/material/Container";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import { useState } from "react";
const theme = createTheme();

export default function SignIn() {
  const [state, setState] = useState({
    isLoggedIn: false,
    isErrorAttempt: false,
  });

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    requesetAuthentification(
      data.get("email") as string,
      data.get("password") as string
    );
  };

  const requesetAuthentification = (name: string, password: string) => {
    const loginData = {
      username: name,
      password: password,
    };

    // Send a POST request to the backend route
    fetch("https://localhost:7211/api/Database/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(loginData),
    })
      .then((response) => {
        if (response.status == 200) {
          // Successful login, navigate to the main page
          console.log(response);
          window.location.href = "/main";
          console.log("reached this");
          setState({
            isLoggedIn: true,
            isErrorAttempt: false,
          });
        } else {
          setState({
            isLoggedIn: false,
            isErrorAttempt: true,
          });
        }
      })
      .catch((error) => {
        // Handle any network or server errors
        console.error("Error:", error);
      });
  };

  return (
    <ThemeProvider theme={theme}>
      <Container component="main" maxWidth="xs">
        <CssBaseline />
        <Box
          sx={{
            marginTop: 8,
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
          }}
        >
          <Typography component="h1" variant="h5">
            Sign in
          </Typography>
          <Box
            component="form"
            onSubmit={handleSubmit}
            noValidate
            sx={{ mt: 1 }}
          >
            <TextField
              margin="normal"
              required
              fullWidth
              id="email"
              label="Email Address"
              name="email"
              autoComplete="email"
              autoFocus
            />
            <TextField
              margin="normal"
              required
              fullWidth
              name="password"
              label="Password"
              type="password"
              id="password"
              autoComplete="current-password"
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
              Sign In
            </Button>
            {state.isErrorAttempt && <div></div>}
          </Box>
        </Box>
      </Container>
    </ThemeProvider>
  );
}
