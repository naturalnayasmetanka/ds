type LocationDTO = {
  id: string;
  name: string;
  address: AddressDTO;
  timezone: string;
  isActive: boolean;
  createdAt: Date;
  updatedAt: Date;
};

type AddressDTO = {
  country: string;
  region: string;
  settlementName: string;
  settlementType: string;
  street: string;
  buildingNumber: string;
  buildingBlock: string | null;
  entrance: string;
  floor: string;
  premiseNumber: string;
  premiseType: string;
  postCode: string;
  fullAddress: string;
  comment: string | null;
};

type CreateLocationDTO = {
  name: string;
  timezone: string;
  adress: {
    country: string;
    region: string;
    settlementName: string;
    settlementType: string;
    street: string;
    buildingNumber: string;
    buildingBlock: string | null;
    entrance: string;
    floor: string;
    premiseNumber: string;
    premiseType: string;
    postCode: string;
    fullAddress: string;
    comment: string | null;
  };
};
