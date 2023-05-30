interface IUser {
  name: string;
  role?: string;
  isAuthenticated: boolean;
}

class User implements IUser {
  name!: string;
  password!: string;
  role?: string;
  isAuthenticated: boolean = false;
}

const AuthenticatedUser = (): User => {
  var user!: User;

  const authenticateUser = (name: string, password: string) => {
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
        if (response.status === 200) {
          user.isAuthenticated = true;
          user.name = name;
        } else {
          user.isAuthenticated = false;
        }
      })
      .catch((error) => {
        // Handle any network or server errors
        console.error("Error:", error);
      });
  };

  const getRole = () => {
    if (!user.isAuthenticated) {
      throw Error();
    }

    fetch("https://localhost:7211/api/Database/login", {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(user),
    })
      .then((response) => {
        if (response.status === 200) {
          response.text().then((x) => (user.role = x));
        } else {
        }
      })
      .catch((error) => {
        // Handle any network or server errors
        console.error("Error:", error);
      });
  };

  return user;
};

export { AuthenticatedUser, User };
