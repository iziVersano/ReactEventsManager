import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { City, CityFormValues } from "../models/city";

export default class CityStore {
    cities: City[] = [];

    constructor() {
        makeAutoObservable(this);
    }

    loadCities = async () => {
        try {
            const cityNames = await agent.Cities.list();
            runInAction(() => {
                this.cities = cityNames.map(name => ({ name })); // Creating instances with properties matching City type
            });
        } catch (error) {
            console.log(error);
        }
    }

    createCity = async (city: CityFormValues) => {
        try {
            await agent.Cities.create(city);
            runInAction(() => {
                this.cities.push({ name: city.name }); // Creating a new city object
            });
        } catch (error) {
            console.log(error);
        }
    }
}
