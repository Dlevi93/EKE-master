export class FormData {
    firstName: string = '';
    lastName: string = '';
    email: string = '';
    birthDate: string = '';
    work: string = '';
    street: string = '';
    city: string = '';
    state: string = '';
    zip: string = '';


    clear() {
        this.firstName = '';
        this.lastName = '';
        this.email = '';
        this.birthDate = '';
        this.work = '';
        this.street = '';
        this.city = '';
        this.state = '';
        this.zip = '';
    }
}

export class Personal {
    firstName: string = '';
    lastName: string = '';
    email: string = '';
    birthDate: string = '';
}

export class Address {
    street: string = '';
    city: string = '';
    state: string = '';
    zip: string = '';
}