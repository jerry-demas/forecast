'use client';

import { Input, Select } from '@headlessui/react';
import axios, { AxiosError, AxiosResponse } from 'axios';
import { useState } from 'react';
import { toast } from 'sonner';
import type { DiceRollResult } from '../interfaces/diceRoll';

//move it this is used in mutliple components.
const diceApi = axios.create({
	baseURL: process.env.NEXT_PUBLIC_API_URL
		? `${process.env.NEXT_PUBLIC_API_URL}/api/diceRoll/`
		: '/api/diceRoll/',
	timeout: 30000,
	withCredentials: true,
});

export interface DiceRollerProps {
	readonly processResultCallback: (result: DiceRollResult) => void;
}

export default function DiceRoller({ processResultCallback }: DiceRollerProps) {
	const [isProcessing, setIsProcessing] = useState(false);
	const [numberOfDice, setnumberOfDice] = useState(1);
	const [typeOfDice, setTypeOfDice] = useState(6);

	const rollDice = () => {
		if (!isProcessing) {
			setIsProcessing(true);
			diceApi.post<number[]>('', { 'numberOfDice': numberOfDice, 'diceSize': typeOfDice })
				.then((response: AxiosResponse<number[]>) => {
					processResultCallback({ resultList: response.data });
				})
				.catch((error: AxiosError<string>) => {
					if (error.status === 400) {
						toast.warning(error.response?.data);
					}
					else {
						toast.error("an error has occured");
						console.log(error);
					}
					processResultCallback({resultList:[]});
				})
				.finally(() => {
					setIsProcessing(false);
				});
		}
	};

	return (
		<div className="">
			<span className="">
				Roll
			</span>
			<span className="pl-5">
				<Input type="number"
					name="numberOfDice"
					className="text-right border data-focus:bg-blue-100 data-hover:shadow"
					value={numberOfDice}
					onChange={e => setnumberOfDice(Number(e.target.value))}
					min={1}
					max={10}
				/>
			</span>
			<span className="pl-5">
				<Select
					name="typeOfDice"
					className="border data-focus:bg-blue-100 data-hover:shadow mb-10"
					aria-label="Type of Dice"
					value={typeOfDice}
					onChange={e => setTypeOfDice(Number(e.target.value))}
				>
					<option value="4">d4</option>
					<option value="6">d6</option>
					<option value="8">d8</option>
					<option value="10">d10</option>
					<option value="12">d12</option>
					<option value="20">d20</option>
				</Select>
			</span>
			<span className="pl-5">
				dice.
			</span>

			<span className="pl-15">
				<button
					onClick={rollDice}
					disabled={isProcessing}
					className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 transition-colors disabled:cursor-not-allowed disabled:opacity-60"
				>
					{isProcessing ? 'ROLLING...' : 'ROLL!'}
				</button>
			</span>
		</div>
	);
}
