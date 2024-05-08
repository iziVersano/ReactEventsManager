import { Button, Container, Dropdown, Image, Menu } from "semantic-ui-react";
import { Link, NavLink } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { useStore } from "../stores/store";

export default observer(function NavBar() {
  const { userStore: { user, logout } } = useStore();

  // Function to check if user is admin
  const isAdmin = user?.roles?.includes("Admin");
  console.log('user', user );
  return (
    <Menu inverted fixed="top">
      <Container>
        <Menu.Item as={NavLink} to="/" header>
          <img src="/assets/logo.png" alt="logo" style={{ marginRight: 10 }} />
          Reactivities
        </Menu.Item>
        <Menu.Item as={NavLink} to="/activities" name="Activities" />
        <Menu.Item as={NavLink} to="/errors" name="Errors" />
        <Menu.Item>
          <Button as={NavLink} to="/createActivity" positive content="Create Activity" />
        </Menu.Item>
        <Menu.Item>
          <Button as={NavLink} to="/createCity" positive content="Create City" />
        </Menu.Item>
        {isAdmin && ( // Render if user is admin
          <Menu.Item>
            <Button as={NavLink} to="/manageUsers" color="teal" content="Manage Users" />
          </Menu.Item>
        )}
        <Menu.Item position="right">
          <Image avatar spaced="right" src={user?.image || "/assets/user.png"} />
          <Dropdown pointing="top left" text={user?.displayName}>
            <Dropdown.Menu>
              <Dropdown.Item as={Link} to={`/profiles/${user?.username}`} text="My Profile" icon="user" />
              <Dropdown.Item onClick={logout} text="Logout" icon="power" />
            </Dropdown.Menu>
          </Dropdown>
        </Menu.Item>
      </Container>
    </Menu>
  );
});
