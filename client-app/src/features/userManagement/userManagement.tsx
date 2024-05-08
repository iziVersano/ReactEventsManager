import { observer } from 'mobx-react-lite';
import { useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Button, Header, Segment, Table } from "semantic-ui-react";
import { useStore } from '../../app/stores/store';


export default observer(function UserManagement() {
    const { userStore } = useStore();
    const { users, getAllUsers } = userStore;

    useEffect(() => {
        getAllUsers();
    }, [getAllUsers]);


    if (!users) return null;
    
    return (
        <Segment clearing>
            <Header content='User Management' sub color='teal' />
            <Table striped selectable>
                <Table.Header>
                    <Table.Row>
                        <Table.HeaderCell>Username</Table.HeaderCell>
                        <Table.HeaderCell>Email</Table.HeaderCell>
                        <Table.HeaderCell>Actions</Table.HeaderCell>
                    </Table.Row>
                </Table.Header>

                <Table.Body>
                    {users.map(user => (
                        <Table.Row key={user.username}>
                            <Table.Cell>{user.username}</Table.Cell>
                            <Table.Cell>{user.email}</Table.Cell>
                            <Table.Cell>
                                <Button as={Link} to={`/editUser/${user.id}`} color='blue' content='Edit' />
                                <Button color='red' content='Delete' />
                            </Table.Cell>
                        </Table.Row>
                    ))}
                </Table.Body>
            </Table>
        </Segment>
    );
});
