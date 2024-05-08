export interface City {
    name: string;
}


export class CityFormValues {
    id?: string = undefined;
    name: string = '';
  
    constructor(city?: CityFormValues) {
      if (city) {
        this.id = city.id;
        this.name = city.name;
      }
    }
  }