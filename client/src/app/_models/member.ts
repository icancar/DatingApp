import { Photo } from "./photo";

export interface Member {
    userName: string;
    photoUrl: string;
    id: number;
    age: number;
    knownAs: string;
    created: Date;
    lastActive: Date;
    gender: string;
    introduction: string;
    lookingFor: string;
    intrests?: any;
    city: string;
    country: string;
    photos: Photo[];
  }
  
