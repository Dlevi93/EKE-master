export class FormData {
    firstName: string = '';
    lastName: string = '';
    email: string = '';
    birthDate: string = '';
    city: string = '';
    country: string = '';
    phoneno: string = '';
    cnp: string = '';

    work: string = '';
    street: string = '';
    state: string = '';
    zip: string = '';


    clear() {
        this.firstName = '';
        this.lastName = '';
        this.email = '';
        this.birthDate = '';
        this.city = '';
        this.country = '';
        this.phoneno = '';
        this.cnp = '';

        this.work = '';
        this.street = '';
        this.state = '';
        this.zip = '';
    }
}

export class Personal {
    firstName: string = '';
    lastName: string = '';
    email: string = '';
    birthDate: string = '';
    city: string = '';
    country: string = '';
    phoneno: string = '';
    cnp: string = '';
}

export class Address {
    street: string = '';
    state: string = '';
    zip: string = '';
}