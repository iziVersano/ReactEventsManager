import { observer } from 'mobx-react-lite';
import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Button, Header, Segment } from "semantic-ui-react";
import LoadingComponent from '../../../app/layout/LoadingComponent';
import { useStore } from '../../../app/stores/store';
import { v4 as uuid } from 'uuid';
import { Formik, Form } from 'formik';
import * as Yup from 'yup';
import MyTextInput from '../../../app/common/form/MyTextInput';
import { CityFormValues } from '../../../app/models/city';

export default observer(function CityForm() {
    const { cityStore } = useStore();
    const { createCity } = cityStore;
    const navigate = useNavigate();

    const [city, setCity] = useState<CityFormValues>(new CityFormValues());

    const validationSchema = Yup.object({
        name: Yup.string().required('City name is required'),
    })

    function handleFormSubmit(city: CityFormValues) {
        let newCity = {
            ...city,
            // Optionally generate an ID for the city
            id: uuid()
        }
        createCity(newCity).then(() => navigate('/'));
    }


    

    return (
        <Segment clearing>
            <Header content='Add New City' sub color='teal' />
            <Formik
                validationSchema={validationSchema}
                initialValues={city}
                onSubmit={values => handleFormSubmit(values)}>
                {({ handleSubmit, isValid, isSubmitting, dirty }) => (
                    <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                        <MyTextInput name='name' placeholder='City Name' />
                        <Button
                            disabled={isSubmitting || !dirty || !isValid}
                            loading={isSubmitting}
                            floated='right'
                            positive
                            type='submit'
                            content='Submit' />
                        <Button as={Link} to='/cities' floated='right' type='button' content='Cancel' />
                    </Form>
                )}
            </Formik>
        </Segment>
    )
})
